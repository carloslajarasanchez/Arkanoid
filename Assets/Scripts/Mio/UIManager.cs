using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //PROBLEMA GORDO: Cuando se reinicia el nivel el UIManager sigue teniendo de referencia los gameobject de antes de cargar la escena al cargarla intenta acceder a estos y no existen
    //Posible solucion: en vez de cargar la escena con LoadScene() reiniciamos el nivel a 1 del levelLoader y cargamos el nivel, el problema es que tendriamos que reiniciar todas las variables de tiempo, vidas y puntuacion
    public static UIManager Instance;

    [Header("Canvas Game")]
    [SerializeField] private TextMeshProUGUI _textPoints;
    [SerializeField] private TextMeshProUGUI _textLifes;
    [SerializeField] private TextMeshProUGUI _textTimer;

    [Header("Canvas GameOver")]
    [SerializeField] private CanvasGroup _gameOverScreen;
    [SerializeField] private float _fadeDuration = .5f;
    [SerializeField] private TextMeshProUGUI _textRanking;

    [Header("Canvas Ready")]
    [SerializeField] private CanvasGroup _readyScreen;
    [SerializeField] private TextMeshProUGUI _textRound;

    private int _currentPoints = 0;     // Valor mostrado actualmente
    private Coroutine _pointsCoroutine; // Para evitar solapamiento de animaciones

    // Start is called before the first frame update
   void Awake()
    {      
            Instance = this;       
    }

    private void Start()
    {
        EventManager.Instance.OnBallLosted += ShowReadyScreen;// Nos suscribimos el evento de perder la bola y mostramos la UI de Ready
        EventManager.Instance.OnLevelFinished += ShowReadyScreen;// Nos suscribimos el evento de terminar el nivel y mostramos UI de Ready
    }

    /// <summary>
    /// Recibe los puntos totales y los muestra con animación incremental.
    /// </summary>
    public void UpdatePoints(int targetPoints)
    {
        // Si hay una animación en curso, la detenemos
        if (_pointsCoroutine != null)
            StopCoroutine(_pointsCoroutine);

        // Iniciamos la nueva animación
        _pointsCoroutine = StartCoroutine(AnimatePoints(targetPoints));
    }

    private IEnumerator AnimatePoints(int targetPoints)
    {
        // Puedes usar interpolación suave o incremento de 1 en 1 (te dejo ambas opciones)
        int start = _currentPoints;
        int difference = targetPoints - start;

        // Si la diferencia es pequeña, usamos incremento 1 a 1 para que se note más fluido
        if (Mathf.Abs(difference) < 50)
        {
            int step = difference > 0 ? 1 : -1;
            while (_currentPoints != targetPoints)
            {
                _currentPoints += step;
                _textPoints.text = _currentPoints.ToString();
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            // Si el salto es grande, usamos una interpolación para que sea más rápido pero suave
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                int displayValue = Mathf.RoundToInt(Mathf.Lerp(start, targetPoints, t));
                _textPoints.text = displayValue.ToString();
                yield return null;
            }

            _currentPoints = targetPoints;
            _textPoints.text = _currentPoints.ToString();
        }

        _pointsCoroutine = null;
    }

    public void UpdateLife(int lifes)
    {
        _textLifes.text = lifes.ToString() + " UP";
    }

    public void UpdateTimer(float time)
    {
        _textTimer.text = FormatTime(time);
    }
    private string FormatTime(float time)
    {
       int minutes = (int)(time / 60f);
       int seconds = (int)(time - minutes * 60f);
       int cents = (int)((time - (int)time) * 100f);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }

    void PrintRanking()
    {
        _textRanking.text = "";// Reseteamos el ranking
        List<ScoreData> ranking = ScoreManager.Instance.LoadRanking();

        for (int i = 0; i < ranking.Count; i++)
        {
            _textRanking.text += $"{i + 1}º   {ranking[i].Level}   |   {FormatTime(ranking[i].Time)}   |   {ranking[i].Score} \n";
        }
    }

    public void ShowGameOverScreen()
    {
        StartGameOverFade(1);
        PrintRanking();
    }

    public void HideGameOverScreen()
    {
        StartGameOverFade(0);
    }

    public void ShowReadyScreen()
    {
        StartReadyFade(1);
        _textRound.text = $"Round {GameManager.Instance.GetLevel().ToString()}";
    }

    public void HideReadyScreen()
    {
        StartReadyFade(0);
    }

    private void StartGameOverFade(float targetAlpha)
    {
        StartCoroutine(FadeCoroutine(targetAlpha, _gameOverScreen));
    }
    private void StartReadyFade(float targetAlpha)
    {
        StartCoroutine(FadeCoroutine(targetAlpha, _readyScreen));
    }

    private IEnumerator FadeCoroutine(float target, CanvasGroup canvas)
    {
        float start = canvas.alpha;
        float time = 0;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(start, target, time / _fadeDuration);

            // Evita interaciones cuando no esta visible
            canvas.interactable = target > 0.5f;
            canvas.blocksRaycasts = target > 0.5f;

            yield return null;
        }

        canvas.alpha = target;
        // Vuelven las interaciones cuando esta visible
        canvas.interactable = target > 0.5f;
        canvas.blocksRaycasts = target > 0.5f;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnBallLosted -= ShowReadyScreen;
    }
}

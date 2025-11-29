using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Canvas Game")]
    [SerializeField] private TextMeshProUGUI _textPoints;// Texto de los puntos
    [SerializeField] private TextMeshProUGUI _textLifes;// Texto de
    [SerializeField] private TextMeshProUGUI _textTimer;// Texto de

    [Header("Canvas GameOver")]
    [SerializeField] private CanvasGroup _gameOverScreen;// Grupo de GameOver
    [SerializeField] private float _fadeDuration = .5f;// Tiempo del fade
    [SerializeField] private TextMeshProUGUI _textRanking;// Texto de

    [Header("Canvas Ready")]
    [SerializeField] private CanvasGroup _readyScreen;// Grupo de ready Screen
    [SerializeField] private TextMeshProUGUI _textRound;// Texto de

    [Header("Tutorial")]
    [SerializeField] private TextMeshProUGUI _textTutorial;// Texto de

    private int _currentPoints = 0;     // Valor mostrado actualmente
    private Coroutine _pointsCoroutine; // Para evitar solapamiento de animaciones

   void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
           // DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        EventManager.Instance.OnBallLosted += ShowReadyScreen;// Nos suscribimos el evento de perder la bola y mostramos la UI de Ready
        EventManager.Instance.OnLevelFinished += ShowReadyScreen;// Nos suscribimos el evento de terminar el nivel y mostramos UI de Ready
        EventManager.Instance.OnBallLaunched += SkipTutorial;// Nos suscribimos el evento de lanzar la bola para saltar el tutorial
        StartCoroutine("ShowTutorial");// Comenzamos la corrutina del tutorial
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
        _textLifes.text = lifes.ToString() + " UP";// Actualiza las vidas
    }

    public void UpdateTimer(float time)
    {
        _textTimer.text = FormatTime(time);// Formatea el tiempo
    }
    //Metodo para formatear el tiempo
    private string FormatTime(float time)
    {
       int minutes = (int)(time / 60f);
       int seconds = (int)(time - minutes * 60f);
       int cents = (int)((time - (int)time) * 100f);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }
    // Metodo para mostrar el ranking
    void PrintRanking()
    {
        _textRanking.text = "";// Reseteamos el ranking
        List<ScoreData> ranking = ScoreManager.Instance.LoadRanking();

        for (int i = 0; i < ranking.Count; i++)
        {
            _textRanking.text += $"{i + 1}º   {ranking[i].Level}   |   {FormatTime(ranking[i].Time)}   |   {ranking[i].Score} \n";
        }
    }
    // Metodo para mostrar la pantalla de GameOver
    public void ShowGameOverScreen()
    {
        StartGameOverFade(1);
        PrintRanking();
    }
    // Metodo para ocular la pantalla de GameOver
    public void HideGameOverScreen()
    {
        StartGameOverFade(0);
    }
    // Metodo para mostrar la pantalla de Ready
    public void ShowReadyScreen()
    {
        StartReadyFade(1);
        _textRound.text = $"Round {GameManager.Instance.Level.ToString()}";
    }
    // Metodo para ocultar la pantalla de Ready
    public void HideReadyScreen()
    {
        StartReadyFade(0);
    }
    // Metodo para empezar el Fade del GameOver
    private void StartGameOverFade(float targetAlpha)
    {
        StartCoroutine(FadeCoroutine(targetAlpha, _gameOverScreen));
    }
    // Metodo para empezar el Fade del Ready
    private void StartReadyFade(float targetAlpha)
    {
        StartCoroutine(FadeCoroutine(targetAlpha, _readyScreen));
    }
    //Corrutina para Fade de paneles.
    private IEnumerator FadeCoroutine(float target, CanvasGroup canvas)
    {
        float start = canvas.alpha;// Alpha inicial
        float time = 0;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;// Aumentamos el tiempo
            canvas.alpha = Mathf.Lerp(start, target, time / _fadeDuration);// Cambiamos el alpha del canvas

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
        EventManager.Instance.OnBallLosted -= ShowReadyScreen;// Nos desuscribimos el evento de terminar de bola perdida
        EventManager.Instance.OnLevelFinished -= ShowReadyScreen;// Nos desuscribimos el evento de terminar el nivel
        EventManager.Instance.OnBallLaunched -= SkipTutorial;// Nos desuscribimos el evento de lanzar la bola
    }

    // Corrutina que muestra el tutorial
    private IEnumerator ShowTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        _textTutorial.text = "Press Space to launch the ball";

        yield return new WaitForSeconds(1.5f);
        _textTutorial.text = "";
    }
    // Corrutina que pasa el tutorial
    private void SkipTutorial()
    {
        StopCoroutine("ShowTutorial");
        _textTutorial.text = "";
        EventManager.Instance.OnBallLaunched -= SkipTutorial;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Canvas Juego")]
    [SerializeField] private TextMeshProUGUI _textPoints;
    [SerializeField] private TextMeshProUGUI _textLifes;
    [SerializeField] private TextMeshProUGUI _textTimer;

    [Header("Canvas GameOver")]
    [SerializeField] private CanvasGroup _gameOverScreen;
    [SerializeField] private float _fadeDuration = .5f;
    [SerializeField] private TextMeshProUGUI _textRanking;

    private int _currentPoints = 0;     // Valor mostrado actualmente
    private Coroutine _pointsCoroutine; // Para evitar solapamiento de animaciones

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
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
       int seconds = (int)(time - minutes / 60f);
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
        StartFade(1);
        PrintRanking();
    }

    public void HideGameOverScreen()
    {
        StartFade(1);
    }

    private void StartFade(float targetAlpha)
    {
        StartCoroutine(FadeCoroutine(targetAlpha));
    }

    private IEnumerator FadeCoroutine(float target)
    {
        float start = _gameOverScreen.alpha;
        float time = 0;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            _gameOverScreen.alpha = Mathf.Lerp(start, target, time / _fadeDuration);

            // Evita interaciones cuando no esta visible
            _gameOverScreen.interactable = target > 0.5f;
            _gameOverScreen.blocksRaycasts = target > 0.5f;

            yield return null;
        }

        _gameOverScreen.alpha = target;
        // Vuelven las interaciones cuando esta visible
        _gameOverScreen.interactable = target > 0.5f;
        _gameOverScreen.blocksRaycasts = target > 0.5f;
    }
}

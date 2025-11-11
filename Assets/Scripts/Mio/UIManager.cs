using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private List<Image> _imagesLife;
    [SerializeField] private TextMeshProUGUI _textPoints;

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



    public void RestLifesUI(int life)
    {
        for(int i = _imagesLife.Count - 1;  i >= life ; i--)
        {
            _imagesLife[i].gameObject.SetActive(false);
        }
    }
}

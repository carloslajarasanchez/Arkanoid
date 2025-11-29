using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private float _timeElapsed = 0;// tiempo trascurrido

    private void Start()
    {
        EventManager.Instance.OnGameFinished += SaveTime;// Se suscribe al evento de juego terminado
        EventManager.Instance.OnLevelRestarted += RestartTime;// Se suscribe al evento de de nivel reiniciado
        EventManager.Instance.OnBallLosted += StopTimer;// Se suscribe al evento de bola perdida
        EventManager.Instance.OnLevelFinished += StopTimer;// Se suscribe al evento de nivel terminado
        EventManager.Instance.OnBallLaunched += StartTimer;// Se suscribe al evento de bola lanzada
    }

    // Metodo para mandar el tiempo al GameManager
    private void SaveTime()
    {
        GameManager.Instance.Time = _timeElapsed;// Actualiza el tiempo del GameManager
    }
    
    //Corrutina para actualizar el tiempo
    private IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(.01f);
            _timeElapsed+=.01f;// Aumenta el tiempo transcurrido
            UIManager.Instance.UpdateTimer(_timeElapsed);// Actualiza el texto del titulo
        }  
    }

    // Reinicia el tiempo
    private void RestartTime()
    {
        _timeElapsed = GameManager.Instance.Time;// Recoge el tiempo del GameManager
        UIManager.Instance.UpdateTimer(_timeElapsed);// Actualiza el tiempo en la UI
    }

    // Metodo que lanza el timer
    private void StartTimer()
    {
        StartCoroutine("UpdateTime");// Inicia la corrutina del actualizar el tiempo
    }
    //Metodo que detiene el timer
    private void StopTimer()
    {
        StopCoroutine("UpdateTime");// Para la corrutina del actualizar el tiempo
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnGameFinished -= SaveTime;// Se desuscribe al evento de de juego terminado
        EventManager.Instance.OnLevelRestarted -= RestartTime;// Se desuscribe al evento de de nivel reiniciado
        EventManager.Instance.OnBallLosted -= StopTimer;// Se desuscribe al evento de bola perdida
        EventManager.Instance.OnLevelFinished -= StopTimer;// Se desuscribe al evento de nivel terminado
        EventManager.Instance.OnBallLaunched -= StartTimer;// Se desuscribe al evento de bola lanzada
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action OnBlockDestroyed;// Evento de bloque destruido
    public event Action OnLevelFinished;// Evento de nivel finalizado
    public event Action OnPowerUpObtained;// Evento de PowerUp obtenido
    public event Action OnBallLosted;// Evento de bola perdida
    public event Action OnGameFinished;// Evento de juego terminado
    public event Action OnLevelRestarted;// Evento de nivel reiniciado
    public event Action OnBallLaunched;// Evento de bola lanzada

    // Patron de singelton
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(Instance);
        }
    }
    /// <summary>
    /// Evento para detectar cuando se rompe un bloque
    /// </summary>
    public void InvokeBlockDestroyed()
    {
        OnBlockDestroyed?.Invoke();
    }
    /// <summary>
    /// Evento para detectar cuando se termina un nivel
    /// </summary>
    public void InvokeLevelFinished()
    {
        OnLevelFinished?.Invoke();
    }
    /// <summary>
    /// Evento para detectar cuando se coge un powerUp
    /// </summary>
    public void InvokePowerUpObtained()
    {
        OnPowerUpObtained?.Invoke();
    }
    /// <summary>
    /// Evento para detectar cuando la bola choca con el limite
    /// </summary>
    public void InvokeBallLosted()
    {
        OnBallLosted?.Invoke();
    }
    /// <summary>
    /// Evento para detectar cuando se termina la partida
    /// </summary>
    public void InvokeGameFinished()
    {
        OnGameFinished?.Invoke();
    }
    /// <summary>
    /// Evento para detectar cuando se reinicia la partida
    /// </summary>
    public void InvokeLevelRestarted()
    {
        OnLevelRestarted?.Invoke();
    }
    /// <summary>
    /// Evento para detectar cuando se lanza la bola
    /// </summary>
    public void InvokeBallLaunched()
    {
        OnBallLaunched?.Invoke();
    }
}

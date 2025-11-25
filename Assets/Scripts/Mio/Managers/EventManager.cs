using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action OnBlockDestroyed;
    public event Action OnLevelFinished;
    public event Action OnPowerUpObtained;
    public event Action OnBallLosted;
    public event Action OnGameFinished;
    public event Action OnLevelRestarted;
    public event Action OnBallLaunched;

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
        //TODO: Implemetar que cuando se consiga un powerUp se desactive el que ya hay y se active el nuevo(para expand, pegajosa, laser)
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
    public void InvokeLevelRestarted()
    {
        OnLevelRestarted?.Invoke();
    }

    public void InvokeBallLaunched()
    {
        OnBallLaunched?.Invoke();
    }
}

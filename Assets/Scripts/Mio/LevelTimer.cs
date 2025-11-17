using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private float _timeElapsed = 0;
    private int minutes, seconds, cents;

    private void Start()
    {
        EventManager.Instance.OnGameFinished += SaveTime;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer() 
    {
        _timeElapsed += Time.deltaTime;
        UIManager.Instance.UpdateTimer(_timeElapsed);
    } 

    private void SaveTime()
    {
        GameManager.Instance.SetTime(_timeElapsed);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnGameFinished -= SaveTime;
    }
    //TODO: metodo para cuando termine el nivel o pierdas el juego mande los datos al gameManager de tiempos
}

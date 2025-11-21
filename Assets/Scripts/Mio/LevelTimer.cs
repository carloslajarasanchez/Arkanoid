using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private float _timeElapsed = 0;

    private void Start()
    {
        EventManager.Instance.OnGameFinished += SaveTime;
        EventManager.Instance.OnLevelRestarted += RestartTime;
        EventManager.Instance.OnBallLosted += StopTimer;
        EventManager.Instance.OnLevelFinished += StopTimer;
        EventManager.Instance.OnBallLaunched += StartTimer;
        

    }
    // Update is called once per frame
    void Update()
    {
        //UpdateTimer();
    }


    private void SaveTime()
    {
        GameManager.Instance.SetTime(_timeElapsed);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnGameFinished -= SaveTime;
    }
    
    private IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(.01f);
            _timeElapsed+=.01f;
            UIManager.Instance.UpdateTimer(_timeElapsed);
        }  
    }

    private void RestartTime()
    {
        _timeElapsed = GameManager.Instance.GetTime();
        UIManager.Instance.UpdateTimer(_timeElapsed);
    }

    private void StartTimer()
    {
        StartCoroutine("UpdateTime");
    }
    private void StopTimer()
    {
        StopCoroutine("UpdateTime");
    }

}

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
        StartCoroutine(UpdateTime());

    }
    // Update is called once per frame
    void Update()
    {
        //UpdateTimer();
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
    
    private IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(.01f);
            _timeElapsed+=.01f;
            UIManager.Instance.UpdateTimer(_timeElapsed);
        }  
    }

}

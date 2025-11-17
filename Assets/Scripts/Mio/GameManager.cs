using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //private static GameManager _instance;
    //public  GameManager Instance
    //{
    //  get
    //  {
            
    //  }
    //  private set;
    //}
    public static GameManager Instance { get; private set; }
    private int _lifes = 3;
    // Para guardar en ranking
    private int _points = 0;
    private float _time=0;
    private int _level=0;

 

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestLifes(int rest)
    {
        if(this._lifes > rest)
        {
            this._lifes -= rest;
            Debug.Log($"Has perdido vida: {_lifes}");
            UIManager.Instance.UpdateLife(_lifes);
        }
        else
        {
            this._lifes = 0; // asegúrate de fijar a cero
            Debug.Log("Has perdido");
            UIManager.Instance.UpdateLife(_lifes);// Actualizamos las vidas
            EventManager.Instance.InvokeGameFinished();// Noticiamos que la partida ha terminado
            ScoreManager.Instance.AddNewScore(_level,_time,_points);// Guardamos la puntuacion en el ranking
            UIManager.Instance.ShowGameOverScreen();// Mostramos la pantalla de GameOver
            //StartCoroutine(ResetLevel());
        }        
    }

    public void AddPoints(int points)
    {
        _points += points;
        UIManager.Instance.UpdatePoints(_points);
    }

    public void AddLifes(int life)
    {        
        _lifes += life;
        UIManager.Instance.UpdateLife(_lifes);
        Debug.Log($"Suma de vida: {_lifes}");
    }

    public void RestartLevel()
    {       
        // De momento resetea a la escena de inicio
        SceneManager.LoadScene(1);
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Metodo para actualizar el tiempo para guardarlo en el ranking
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        this._time = time;
    }
    /// <summary>
    /// Metodo para actualizar el nivel para guardarlo en el ranking
    /// </summary>
    /// <param name="level">Nivel al que ha llegado el jugador</param>
    public void SetLevel(int level)
    {
        this._level = level;
    }
}

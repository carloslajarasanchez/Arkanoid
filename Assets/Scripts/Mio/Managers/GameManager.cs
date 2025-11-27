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
    private int _balls = 1;

    public int Lifes { get { return _lifes; } set { _lifes = value; } }
    public int Points { get { return _points; } set { _points = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public float Time { get { return _time; } set { _time = value; } }
    public int Balls { get { return _balls; } set {_balls = value; } }

 

    // Start is called before the first frame update
    void Awake()
    {
        //Patron de singelton
        if(Instance != null && Instance !=this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
    }

    // Metodo para restar vidas
    public void RestLifes(int rest)
    {
        // Si las vidas son mayores que lo que se le quita
        if(this._lifes > rest)
        {
            this._lifes -= rest;// Restamos las vidas          
            UIManager.Instance.UpdateLife(_lifes);// Actualizamos las vidas en la UI
            Debug.Log($"Has perdido vida: {_lifes}");
        }
        else
        {
            this._lifes = 0; // Aseguramos que la vida es cero
       
            UIManager.Instance.UpdateLife(_lifes);// Actualizamos las vidas
            EventManager.Instance.InvokeGameFinished();// Noticiamos que la partida ha terminado
            ScoreManager.Instance.AddNewScore(_level,_time,_points);// Guardamos la puntuacion en el ranking
            UIManager.Instance.ShowGameOverScreen();// Mostramos la pantalla de GameOver

            Debug.Log("Has perdido");
        }        
    }

    // Metodo para sumar puntos
    public void AddPoints(int points)
    {
        _points += points;// Aumentamos los puntos
        UIManager.Instance.UpdatePoints(_points);// Actualizamos los puntos en la UI
    }

    // Metodo para sumar vidas
    public void AddLifes(int life)
    {        
        _lifes += life;// Aumentamos la vida
        UIManager.Instance.UpdateLife(_lifes);// Actualizamos la vida en la UI
        Debug.Log($"Suma de vida: {_lifes}");
    }

    public void RestartLevel()
    {
        // De momento resetea a la escena de inicio (Cutrez)
        this._lifes = 3;
        this._points = 0;
        this._time = 0;
        this._level = 0;
        UIManager.Instance.HideGameOverScreen();// Escondemos el panel de GameOver
        UIManager.Instance.UpdateLife(_lifes);// Actualizamos las vidas de la UI
        UIManager.Instance.UpdatePoints(_points);// Actualizamos los puntos de la UI
        EventManager.Instance.InvokeLevelRestarted();// Invocamos el metodo de reiniciar el nivel
    }

    // Metodo para el boton de volver a la escena de titulo
    public void ExitMenu()
    {
        SceneManager.LoadScene("TitleMenu");// Carga la escena del Menu principal
    }

}

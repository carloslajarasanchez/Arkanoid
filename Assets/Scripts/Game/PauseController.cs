using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private bool isPaused = false;// Boleano que controla si se esta en pausa
    [SerializeField] private GameObject pausePanel;// Panel de la pausa

    void Update()
    {
        PauseGame();
    }

    // Metodo que pausa el juego
    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))// Si se pulsa escape
        {
            if (!isPaused)// Si no esta pausado pausa el juego
            {
                Time.timeScale = 0f;// Paramos el tiempo
                isPaused = true;// Cambiamos a true el estado de pausa
                pausePanel.SetActive(true);// Activamos el panel de Pausa
            }
            else// Si no esta pausado lo continua
            {
                ResumeGame();// Continuamos con el nivel
            }
        }  
    }
    // Meotodo para volver a la pantalla del titulo
    public void ExitGame()
    {
        Time.timeScale = 1f;// Reanudamos el tiempo
        isPaused = false;// Cambiamos a false el estado de pausa
        SceneManager.LoadScene("TitleMenu");// Cargamos la escena del titulo
    }

    // Metodo para quitar la pausa
    public void ResumeGame()
    {
        Time.timeScale = 1f;// Reanudamos el tiempo
        isPaused = false;// Cambiamos a false el estado de pausa
        pausePanel.SetActive(false);// Desactivamos el panel de Pausa
    }
}

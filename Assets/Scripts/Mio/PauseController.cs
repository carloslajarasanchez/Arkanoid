using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                isPaused = true;
                pausePanel.SetActive(true);
            }
            else
            {
                ResumeGame();
            }
        }  
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("TitleMenu");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
    }
}

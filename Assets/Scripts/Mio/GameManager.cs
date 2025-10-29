using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int _lifes = 3;


    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
            Debug.Log($"Vidas restantes: {_lifes}");
            UIManager.Instance.RestLifesUI(_lifes);
        }
        else
        {
            this._lifes = 0; // asegúrate de fijar a cero
            Debug.Log("Has perdido");
            UIManager.Instance.RestLifesUI(_lifes);
            StartCoroutine(ResetLevel());
        }        
    }

    IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(3f);
        Scene actualScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(actualScene.name);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public List<Image> ImagesLife;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
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

    public void RestLifesUI(int life)
    {
        for(int i = ImagesLife.Count - 1;  i >= life ; i--)
        {
            ImagesLife[i].gameObject.SetActive(false);
        }
    }
}

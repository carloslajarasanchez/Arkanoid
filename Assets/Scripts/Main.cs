using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainClass
{
    public static GameManager GM;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Main()
    {
        //GM = new GameManager();
        Debug.Log("Me ejecuto antes de cargar cualquier escena");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubo : MonoBehaviour
{
    public GameObject ObjetoACambiar;
    // Start is called before the first frame update
    void Start()
    {
        CambiarColorComponent cambiarColor = this.ObjetoACambiar.GetComponent<CambiarColorComponent>();
        //this.ObjetoACambiar.GetComponent<CambiarColorComponent>().CambiarColor(Color.green);
        cambiarColor.CambiarColor(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

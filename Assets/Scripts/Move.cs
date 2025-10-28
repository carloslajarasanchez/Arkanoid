using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    [SerializeField] private float _velocity = 2f;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoverPala();
    }

    private void MoverPala()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // Necesitamos sumar a nuestra posicion el movimiento horizontal pq si no lo hacemos el personaje vuelve a su posicion inicial y no es lo que queremos
        //this.transform.position = new Vector3(transform.position.x + (moveHorizontal * _velocity * Time.deltaTime), this.transform.position.y, this.transform.position.z);
        //Mueve directamente solo pasandole el vector, no hace falta pasar la posicion del gameObject
        Vector3 direction = new Vector3();
        //Vector3 direction2 = new Vector3(moveHorizontal * _velocity * Time.deltaTime, 0,0);

        //direction.x = moveHorizontal;
        //direction.y = moveVertical;
        direction.y = 0;
        direction.z = 0;
        //direction.Normalize();
        direction.x = Mathf.Clamp(direction.x + (moveHorizontal * _velocity * Time.deltaTime), -7.5f, 7.5f);
        this.transform.Translate(direction);
    }
}

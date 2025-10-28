using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocity = 2f;
    [SerializeField] private float bounds = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoverLimits();
    }

    private void MoverLimits()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {

            float direction = Input.GetAxis("Horizontal");// Guardamos el valor del input horizontal del jugador

            Vector2 playerPosition = transform.position;// Guardamos la posicion del jugador
            playerPosition.x = Mathf.Clamp(playerPosition.x + (direction * velocity * Time.deltaTime), -bounds, bounds);// Asignamos el movimiento a la posicion en x del jugador con limites
            transform.position = playerPosition;// Actualizamos el transform del jugador
        }
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _velocity = 7f;

    [Header("Limits")]
    [SerializeField]private GameObject _wallRight;
    [SerializeField]private GameObject _wallLeft;

    private float _minLimit;
    private float _maxLimit;
 

    // Start is called before the first frame update
    void Start()
    {
        CalculateLimits();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {

            float direction = Input.GetAxis("Horizontal");// Guardamos el valor del input horizontal del jugador

            Vector2 playerPosition = transform.position;// Guardamos la posicion del jugador
            playerPosition.x = Mathf.Clamp(playerPosition.x + (direction * _velocity * Time.deltaTime), _minLimit, _maxLimit);// Asignamos el movimiento a la posicion en x del jugador con limites
            transform.position = playerPosition;// Actualizamos el transform del jugador
        }
    }

    private void CalculateLimits()
    {
        _minLimit = _wallLeft.transform.position.x + _wallLeft.transform.localScale.x/2 + transform.localScale.x/2;
        _maxLimit = _wallRight.transform.position.x - _wallRight.transform.localScale.x/2 - transform.localScale.x/2;
    }
}

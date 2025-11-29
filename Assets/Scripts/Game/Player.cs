using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _velocity = 7f;// Velocidad del player

    [Header("Limits")]
    [SerializeField]private GameObject _wallRight;// Referencia a la pared derecha
    [SerializeField]private GameObject _wallLeft;// Referencia a la pared derecha

    [Header("Sound Clips")]
    [SerializeField] private AudioClip _respawnClip;// Sonido de respawn del player para usarse con el AudioManager

    private Animator _playerAnimator;// Referencia al animador del player
    private float _minLimit;// Limite minimo de movimiento en x
    private float _maxLimit;// Limite maximo de movimiento en x
    private Vector2 _initialPosition;// Posicion inicial del player
    private bool _canMove;// Controla si el player puede moverse
    private List<PowerUp> actualPowerUp = new List<PowerUp>();// Lista de los powerUps del player que tiene activos
 

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;// Guardamos la posicion inicial
        _playerAnimator = GetComponent<Animator>();// Cogemos la referencia al animator
        CalculateLimits();// Calculamos los limites del player con su tamaño actual

        EventManager.Instance.OnLevelFinished += ResetToInitialPosition;// Nos suscribimos al evento de nivel finalizado para reiniciar la posicion
        EventManager.Instance.OnLevelFinished += LevelCompleteRespawnPlayer;// Nos suscribimos al evento de nivel finalizado para respawnear al player
        EventManager.Instance.OnBallLosted += DestroyPlayerAnimation;// Nos suscribimos al evento de perder la bola para ejecutar animacion de destruccion
    }

    // Update is called once per frame
    void Update()
    {
        // Si se puede mover
        if (_canMove)
        {
            Move();// Ejecuta el metodo de moverse
        }
        
    }

    // Metodo para mover al player
    private void Move()
    {
        // Si el input en x es distinto de 0 
        if (Input.GetAxisRaw("Horizontal") != 0)
        {

            float direction = Input.GetAxis("Horizontal");// Guardamos el valor del input horizontal del jugador

            Vector2 playerPosition = transform.position;// Guardamos la posicion del jugador
            playerPosition.x = Mathf.Clamp(playerPosition.x + (direction * _velocity * Time.deltaTime), _minLimit, _maxLimit);// Asignamos el movimiento a la posicion en x del jugador con limites
            transform.position = playerPosition;// Actualizamos el transform del jugador
        }
    }

    // Metodo para calcular los limites de las paredes
    public void CalculateLimits()
    {
        _minLimit = _wallLeft.transform.position.x + _wallLeft.transform.localScale.x/2 + transform.localScale.x/2;// calculamos el limite izquierdo [posicion del muro en x - escala muro/2 + escalaJugador/2]
        _maxLimit = _wallRight.transform.position.x - _wallRight.transform.localScale.x/2 - transform.localScale.x/2;// calculamos el limite derecho [posicion del muro en x - escala muro/2 + escalaJugador/2]
    }

    // Metodo para reiniciar la posicion inicial
    public void ResetToInitialPosition()
    {
        transform.position = _initialPosition;// Reiniciamos a la posicion inicial
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            PowerUp powerUp = collision.gameObject.GetComponent<PowerUp>();// Guardamos el powerUp recogido

            if (powerUp.GetStakeable())// Si es stakeable
            {              
                powerUp.ApplyEffect();// Aplicamos su efecto
                actualPowerUp.Add(powerUp);// Lo añadimos a la lista de powerUps
                collision.gameObject.SetActive(false);// Desactivamos el powerUp
            }
            else// Si no es stakeable
            {               
                foreach (PowerUp x in actualPowerUp) // Recorremos la lista
                {
                    x.Remove();// Ejecutamos su metodo Remove
                }
                powerUp.ApplyEffect();// Aplicamos su efecto
                actualPowerUp.Add(powerUp);// Añadimos el PowerUp a la lista
            }
        }
    }

    // Este metodo esta suscrito al evento ed OnBallLosted para que se ejecute la animacion de destruir al player
    private void DestroyPlayerAnimation()
    {
        _canMove = false;// Cancelamos su movimiento
        _playerAnimator.SetTrigger("TriggerLostBall");// Ejecutamos animacion de perder la bola
    }

    // Este metodo se llama desde un animation event para saber cuando ha terminado la animacion de respawn del player
    public void CanMove()
    {
        _canMove = true;// Permitimos su movimiento 
        UIManager.Instance.HideReadyScreen();// Ocultamos la UI de Ready
    }

    // Este metodo se llama desde un animation event cuando empieza la animacion de respawn del player
    public void PlayRespawnSound()
    {
        AudioManager.Instance.PlaySound(_respawnClip);// Ejecutamos el sonido de respawn
    }

    private void LevelCompleteRespawnPlayer()
    {
        _playerAnimator.SetTrigger("TriggerLevelFinished");// Ejecutamos la animacion de nivel Finalizado
    }

    //TODO: Arreglar que la nave vuelve a sonar una vez se termina la partida ya que se ejecuta la animacion de destruirse y luego pasa a la de respawn invocando el sonido
    private void UnsubscribeLostBall()
    {
        EventManager.Instance.OnBallLosted -= DestroyPlayerAnimation;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnLevelFinished -= ResetToInitialPosition;// Nos desuscribimos al evento de nivel finalizado
        EventManager.Instance.OnBallLosted -= DestroyPlayerAnimation;
        EventManager.Instance.OnLevelFinished -= LevelCompleteRespawnPlayer;// Nos suscribimos al evento de nivel finalizado
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _velocity = 7f;

    [Header("Limits")]
    [SerializeField]private GameObject _wallRight;
    [SerializeField]private GameObject _wallLeft;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip _respawnClip;

    private Animator _playerAnimator;
    private float _minLimit;
    private float _maxLimit;
    private Vector2 _initialPosition;
    private bool _canMove;
    private PowerUp actualPowerUp;
 

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _playerAnimator = GetComponent<Animator>();
        CalculateLimits();
        EventManager.Instance.OnLevelFinished += ResetToInitialPosition;// Nos suscribimos al evento de nivel finalizado
        EventManager.Instance.OnLevelFinished += LevelCompleteRespawnPlayer;// Nos suscribimos al evento de nivel finalizado
        EventManager.Instance.OnBallLosted += DestroyPlayerAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            Move();
        }
        
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

    public void CalculateLimits()
    {
        _minLimit = _wallLeft.transform.position.x + _wallLeft.transform.localScale.x/2 + transform.localScale.x/2;
        _maxLimit = _wallRight.transform.position.x - _wallRight.transform.localScale.x/2 - transform.localScale.x/2;
    }

    public void ResetToInitialPosition()
    {
        transform.position = _initialPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            if(actualPowerUp != null)// Si hay un powerUp activo se aplica el metodo revertir powerUp
            {
                actualPowerUp.RevertEffect();
                //Destroy(actualPowerUp);
            }

            PowerUp powerUp = collision.gameObject.GetComponent<PowerUp>();// Guardamos el powerUp recogido

            if (powerUp != null)// Si no esta vacio
            {
                actualPowerUp = Instantiate(powerUp);// Lo asignamos como powerUp actual
                powerUp.ApplyEffect();// Aplicamos su efecto
                Destroy(collision.gameObject);// Destruimos el powerUp
            }
        }
    }

    // Este metodo esta suscrito al evento ed OnBallLosted para que se ejecute la animacion de destruir al player
    private void DestroyPlayerAnimation()
    {
        _canMove = false;
        _playerAnimator.SetTrigger("TriggerLostBall");
    }

    // Este metodo se llama desde un animation event para saber cuando ha terminado la animacion de respawn del player
    public void CanMove()
    {
        _canMove = true;
        UIManager.Instance.HideReadyScreen();// Ocultamos la UI de Ready
    }

    // Este metodo se llama desde un animation event cuando empieza la animacion de respawn del player
    public void PlayRespawnSound()
    {
        AudioManager.Instance.PlaySound(_respawnClip);
    }

    private void LevelCompleteRespawnPlayer()
    {
        _playerAnimator.SetTrigger("TriggerLevelFinished");
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

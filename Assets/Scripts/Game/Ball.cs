using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private Vector2 _initialDirection = new Vector2(1, 2);// Variable que controla la direccion de lanzamiento de la bola
    [SerializeField] private float velocity = 5f;// Variable para la velocidad de la bola

    private Rigidbody2D _rb2D;// Rigidbody2D de la bola
    private bool _launched = false;// Variable para controlar si la bola se ha lanzado
    private bool _restared = false;// Variable para controlar que la bola no se lance cuando se haya perdido
    private bool _isClone = false;

    public bool IsClone { get { return _isClone; } set { _isClone = value; } }


    [Header("Dependencias")]
    [SerializeField]private GameObject _launchParent;// Referencia a la pala

    [Header("Sonidos")]
    [SerializeField] private AudioClip _collisionSound;
    [SerializeField] private AudioClip _playerSound;
    [SerializeField] private AudioClip _loseBallSound;

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();// Seteamos la variable Rigidbody de la bola para poder modificarla
        //_launchParent = GameObject.FindGameObjectWithTag("Player");
        if (!_isClone)
        {
            ResetBall();// Reseteamos la posición de la bola a la de la Pala
            transform.SetParent(_launchParent.transform, true);// Convertimos a la Pala en padre de la bola para que siga su movimiento
        }      

        EventManager.Instance.OnLevelFinished += ResetBall;// Nos suscribimos el evento de nivel finalizado
        EventManager.Instance.OnGameFinished += CanLauch;
        EventManager.Instance.OnLevelRestarted += CanLauch;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchBall();// Lanzamos bola
    }

    // Metodo para lanzar la bola
    private void LaunchBall()
    {
        if (!_launched && !_restared)// Si no se ha lanzado todavia la bola
        {
            if (Input.GetKeyDown(KeyCode.Space))// Y se presiona espacio
            {
                transform.parent = null;// Hacemos que la Pala deje de ser el hijo de la bola
                _rb2D.velocity = _initialDirection.normalized * velocity;// Aplicamos a la bola una velocidad hacia la direccion inicial que decidimos
                //_rb2D.AddForce(_initialDirection * velocity);// Aplicamos a la bola una fuerza hacia la direccion inicial que decidimos
                _launched = true;// Para que a la bola no se le pueda volver a aplicar otra fuerza hasta que no se reinicie
                EventManager.Instance.InvokeBallLaunched();// Invocamos el evento de lanzar la bola.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limit"))// Comparamos si ha atravesado los limites
        {
            if(GameManager.Instance.Balls <= 1)// Si el numero de bolas es 1 o menor
            {
                GameManager.Instance.RestLifes(1);// Llamamos al GameManager para restar vidas del jugador
                AudioManager.Instance.PlaySound(_loseBallSound);
                EventManager.Instance.InvokeBallLosted();// Activamos el evento de bola perdida
                ResetBall();// Reiniciamos la posicion de la bola
            }
            else// Si el numero es mayor
            {
                if (IsClone)// Comprobamos si es un clon de la bola
                {
                    Destroy(this.gameObject);// Destruimos el clon
                }
                
                GameManager.Instance.Balls--;// Bajamos el numero de bolas
                Debug.Log($"Bolas restantes: {GameManager.Instance.Balls}");
            }            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _launched)
        {
            ControlLaunch(collision);

            AudioManager.Instance.PlaySound(_playerSound);
        }
        else
        {
            AudioManager.Instance.PlaySound(_collisionSound);// Esta en else para evitar que se ejecute el sonido de player y las demas colisiones juntos
        }

            VelocityFix(); // Corrección de rebotes verticales/horizontales
        
    }

    private void ControlLaunch(Collision2D collision)
    {
        // Obtener punto de contacto
        Vector2 contactPoint = collision.GetContact(0).point;// Obtenemos el punto de contacto de la bola sobre la pala
        Vector2 paddleCenter = collision.transform.position;// Obtenemos el centro de la pala

        // Calcular offset horizontal respecto al centro de la pala
        float offset = contactPoint.x - paddleCenter.x;// Obtenemos la distancia horizontal entre el punto de impacto y la pala (si es positivo: parte derecha, si es negativo: parte izquierda)
        float paddleWidth = collision.collider.bounds.size.x;// El ancho total de la pala

        // Normalizar offset (-1 a 1)
        float normalizedOffset = Mathf.Clamp(offset / (paddleWidth / 2f), -1f, 1f);

        // Ángulo máximo de rebote en grados
        float maxBounceAngle = 75f;// Para establecer el angulo máximo de rebote
        float bounceAngle = normalizedOffset * maxBounceAngle;
        float bounceAngleRad = bounceAngle * Mathf.Deg2Rad;

        // Nueva dirección basada en el ángulo
        Vector2 newDirection = new Vector2(Mathf.Sin(bounceAngleRad), Mathf.Cos(bounceAngleRad)).normalized;

        // Aplicar nueva velocidad
        _rb2D.velocity = newDirection * velocity;
    }


    // Metodo para resetear la posicion de la bola a la Pala
    public void ResetBall()
    {
        // Para evitar que se queden bolas residuales
        if (IsClone)
        {
            Destroy(this.gameObject);
        }
        gameObject.SetActive(true);
        _rb2D.velocity = Vector2.zero;// Seteamos la velocidad de la bola a cero para evitar que se le siga aplicando una fuerza
        _launched = false;// Cambiamos para poder volver a lanzar la bola

        Vector2 ballPosition = new Vector2();// Creamos un Vector2 
        ballPosition.y = _launchParent.transform.position.y + _launchParent.transform.localScale.y / 2 + transform.localScale.y / 2;// Calculamos la posicion en Y respecto a la pala
        ballPosition.x = _launchParent.transform.position.x;// Calculamos la posicion X respecto a la Pala
        transform.position = ballPosition;// Asignamos al transform el Vector2 que hemos calculado
        transform.SetParent(_launchParent.transform, true);// Volvemos a emparentar la bola a la Pala para que siga su movimiento

        
    }

    // Metodo para arreglar el movimiento de la bola cuando se queda rebotando de forma paralela o vertical
    private void VelocityFix()
    {
        float velocityDelta = 0.5f;
        float minVelocity = 0.2f;

        if (_launched) // Esta condicion controla que no se aplique fuerzas cuando la bola este pegada a la pala
        {
            if (Mathf.Abs(_rb2D.velocity.x) < minVelocity)// Si la velocidad absoluta en x es menor que la minima velocidad
            {
                velocityDelta = Random.value < .5f ? velocityDelta : -velocityDelta;
                _rb2D.velocity += new Vector2(velocityDelta, 0f);// Agregamos una pequeña variacion horizontal y puede salir aleatoriamente hacia la derecha o izquierda
            }

            if (Mathf.Abs(_rb2D.velocity.y) < minVelocity)// Si la velocidad absoluta en y es menor que la minima velocidad
            {
                velocityDelta = Random.value < .5f ? velocityDelta : -velocityDelta;
                _rb2D.velocity += new Vector2(0f, velocityDelta);// Agregamos una pequeña variacion vertical y puede salir aleatoriamente hacia la derecha o izquierda
            }
        }
        
    }

    private void CanLauch()
    {

        _restared = !_restared;
    }
    private void OnDestroy()
    {
        EventManager.Instance.OnLevelFinished -= ResetBall;// Nos desuscribimos al evento de nivel finalizado
    }
}

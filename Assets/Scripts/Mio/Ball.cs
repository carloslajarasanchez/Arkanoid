using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private Vector2 _initialDirection = new Vector2(1, 2);// Variable que controla la direccion de lanzamiento de la bola
    [SerializeField] private float velocity = 5f;// Variable para la velocidad de la bola

    private Rigidbody2D _rb2D;// Rigidbody2D de la bola
    private bool _launched = false;// Variable para controlar si la bola se ha lanzado

    [Header("Dependencias")]
    [SerializeField]private GameObject _launchParent;// Referencia a la pala

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();// Seteamos la variable Rigidbody de la bola para poder modificarla
        //_launchParent = GameObject.FindGameObjectWithTag("Player");
        ResetBall();// Reseteamos la posición de la bola a la de la Pala
        transform.SetParent(_launchParent.transform, true);// Convertimos a la Pala en padre de la bola para que siga su movimiento
        
    }

    // Update is called once per frame
    void Update()
    {
        LaunchBall();// Lanzamos bola
    }

    // Metodo para lanzar la bola
    private void LaunchBall()
    {
        if (!_launched)// Si no se ha lanzado todavia la bola
        {
            if (Input.GetKeyDown(KeyCode.Space))// Y se presiona espacio
            {
                transform.parent = null;// Hacemos que la Pala deje de ser el hijo de la bola
                //_rb2D.velocity = _initialDirection * velocity;// Aplicamos a la bola una velocidad hacia la direccion inicial que decidimos
                _rb2D.AddForce(_initialDirection * velocity);// Aplicamos a la bola una fuerza hacia la direccion inicial que decidimos
                _launched = true;// Para que a la bola no se le pueda volver a aplicar otra fuerza hasta que no se reinicie
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limit"))// Comparamos si ha atravesado los limites
        {
            _rb2D.velocity = Vector2.zero;// Seteamos la velocidad de la bola a cero para evitar que se le siga aplicando una fuerza
            _launched = false;// Cambiamos para poder volver a lanzar la bola
            GameManager.Instance.RestLifes(1);// Llamamos al GameManager para restar vidas del jugador
            ResetBall();// Reiniciamos la posicion del jugador
        }
       VelocityFix();
    }

    // Metodo para resetear la posicion de la bola a la Pala
    private void ResetBall()
    {
        Vector2 ballPosition = new Vector2();// Creamos un Vector2 
        ballPosition.y = _launchParent.transform.position.y + _launchParent.transform.localScale.y / 2 + transform.localScale.y / 2;// Calculamos la posicion en Y respecto a la pala
        ballPosition.x = _launchParent.transform.position.x;// Calculamos la posicion X respecto a la Pala
        transform.position = ballPosition;// Asignamos al transform el Vector2 que hemos calculado
        transform.SetParent(_launchParent.transform, true);// Volvemos a emparentar la bola a la Pala para que siga su movimiento

        _rb2D.velocity = Vector2.zero;// Seteamos la velocidad de la bola a cero para evitar que se le siga aplicando una fuerza

    }

    // Metodo para arreglar el movimiento de la bola cuando se queda rebotando de forma paralela o vertical
    private void VelocityFix()
    {
        float velocityDelta = 0.5f;
        float minVelocity = 0.2f;

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

    //para saber en que punto golpea la bola en la pala
    //para calcular el movimiento de la bola habria calcular en que posicion colisiona
    // var contactpoint = collision.GetContact[0]
    //var point = contactpoint.point;
    //this.transform.InverseTransformPoint(point)
}

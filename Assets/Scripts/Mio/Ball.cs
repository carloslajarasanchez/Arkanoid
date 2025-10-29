using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private Vector2 _initialDirection = new Vector2(1, 2);
    [SerializeField] private float velocity = 5f;

    private Rigidbody2D _rb2D;
    private bool _launched = false;
    private Vector2 _initialPosition;

    [Header("Dependencias")]
    [SerializeField]private GameObject _launchParent;
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;
        transform.SetParent(_launchParent.transform, true);
        
    }

    // Update is called once per frame
    void Update()
    {
        LaunchBall();
    }

    private void LaunchBall()
    {
        if (!_launched)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Entro Espacio");
                transform.parent = null;
                _rb2D.AddForce(_initialDirection * velocity);

                _launched = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limit"))
        {
            _rb2D.velocity = Vector2.zero;
            _launched = false;
            GameManager.Instance.RestLifes(1);
            ResetBall();
        }
    }

    private void ResetBall()
    {
        Vector2 ballPosition = new Vector2();
        ballPosition.y = _launchParent.transform.position.y + _launchParent.transform.localScale.y / 2 + transform.localScale.y / 2;
        ballPosition.x = _launchParent.transform.position.x;
        transform.position = ballPosition;
        transform.SetParent(_launchParent.transform, true);
        
    }
}

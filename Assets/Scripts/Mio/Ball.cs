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
    public GameObject LaunchParent;
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        transform.SetParent(LaunchParent.transform, true);
        _initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
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
            SceneManager.LoadSceneAsync(0);
        }
    }
}

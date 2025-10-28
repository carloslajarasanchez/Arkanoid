using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private Vector2 initialDirection = new Vector2(1, 2);
    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

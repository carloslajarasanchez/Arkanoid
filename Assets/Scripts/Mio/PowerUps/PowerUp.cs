using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected float fallSpeed = 2f;

    void Update()
    {
        // Movimiento hacia abajo
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Limit")) // Límite inferior
        {
            Destroy(gameObject);
        }
    }

    // Cada power-up implementa su propio efecto
    public abstract void ApplyEffect();
}

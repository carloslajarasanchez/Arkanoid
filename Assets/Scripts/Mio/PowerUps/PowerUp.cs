using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected float fallSpeed = 2f;

    protected virtual void Start()
    {
        EventManager.Instance.OnBallLosted += DestroyPowerUp;// Nos suscribimos al evento de cuando perdemos una bola
    }
    void Update()
    {
        // Movimiento hacia abajo
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Limit")) // Límite inferior
        {
            Destroy(this.gameObject);
        }
    }

    // Cada power-up implementa su propio efecto
    public abstract void ApplyEffect();

    // Este metodo destruye los powerUp al perder la bola
    private void DestroyPowerUp()
    {
        Destroy(this.gameObject);
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnBallLosted -= DestroyPowerUp;// Nos desuscribimos del evento
    }
}

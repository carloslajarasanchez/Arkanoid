using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected float fallSpeed = 2f;// Velocidad de caida
    protected bool stackeable;// Si es stakeable

    protected virtual void Start()
    {
        EventManager.Instance.OnBallLosted += DestroyPowerUp;// Nos suscribimos al evento de cuando perdemos una bola
        EventManager.Instance.OnBallLosted += Remove;// Nos suscribimos al evento de cuando perdemos una bola
        EventManager.Instance.OnLevelFinished += DestroyPowerUp;// Nos suscribimos al evento de cuando terminamos un nivel
    }
    void Update()
    {       
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);// Movimiento hacia abajo
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
    public abstract void Remove();

    // Este metodo destruye los powerUp al perder la bola
    private void DestroyPowerUp()
    {
        Remove();
        Destroy(this.gameObject);
    }

    public bool GetStakeable()
    {
        return this.stackeable;
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnBallLosted -= DestroyPowerUp;// Nos desuscribimos del evento
        EventManager.Instance.OnLevelFinished -= DestroyPowerUp;
    }
}

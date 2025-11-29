using UnityEngine;

public class PowerUpExpandPaddle : PowerUp
{
    [SerializeField] private float scaleMultiplier = 1.5f;// Escala de multiplicacion de tamaño
    private GameObject _player;// Referencia del player
    private Vector3 _playerOriginalSize = new Vector3(2,.5f,1);// Tamaño original del player para revertir el efecto
    
    protected new void Start()
    {
        base.Start(); // Esto asegura que se suscriba al evento
        _player = FindAnyObjectByType<Player>().gameObject;// Conseguimos la referencia del player
        stackeable = false;// Este powerUp no es stackeable
    }
    public override void ApplyEffect()
    {
        Debug.Log("Power Up ExpandPaddle");
        // Aplicamos un aumento al tamaño del player
        // TODO: Cambiar sprite a mas largo
        _player.transform.localScale = new Vector3(
            _player.transform.localScale.x * scaleMultiplier,
            _player.transform.localScale.y,
            _player.transform.localScale.z
        );

        _player.GetComponent<Player>().CalculateLimits();// Calculamos los limites del player con las paredes
    }

    public override void Remove()
    {
        _player.transform.localScale = _playerOriginalSize;// Devolvemos al player su tamaño original
        _player.GetComponent<Player>().CalculateLimits();// Calculamos los limites del player
        
    }
}


using UnityEngine;

public class PowerUpExpandPaddle : PowerUp
{
    [SerializeField] private float scaleMultiplier = 1.5f;
    private GameObject _player;
    
    protected new void Start()
    {
        base.Start(); // Esto asegura que se suscriba al evento
        _player = FindAnyObjectByType<Player>().gameObject;
    }
    public override void ApplyEffect()
    {
        // Hay un problemilla con esto y que si la pala coger el power up con la bola, la bola crece tambien
        Debug.Log("Power Up ExpandPaddle");
        _player.transform.localScale = new Vector3(
            _player.transform.localScale.x * scaleMultiplier,
            _player.transform.localScale.y,
            _player.transform.localScale.z
        );

        _player.GetComponent<Player>().CalculateLimits();
    }
}


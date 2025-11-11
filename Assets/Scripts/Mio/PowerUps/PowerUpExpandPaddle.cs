using UnityEngine;

public class PowerUpExpandPaddle : PowerUp
{
    [SerializeField] private float scaleMultiplier = 1.5f;
    private GameObject _player;
    
    private void Start()
    {
        _player = FindAnyObjectByType<Player>().gameObject;
    }
    public override void ApplyEffect()
    {
        _player.transform.localScale = new Vector3(
            _player.transform.localScale.x * scaleMultiplier,
            _player.transform.localScale.y,
            _player.transform.localScale.z
        );

        _player.GetComponent<Player>().CalculateLimits();
    }
}


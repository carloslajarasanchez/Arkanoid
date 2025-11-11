using UnityEngine;

public class PowerUpExpandPaddle : PowerUp
{
    [SerializeField] private float scaleMultiplier = 1.5f;

    protected override void ApplyEffect(GameObject player)
    {
        player.transform.localScale = new Vector3(
            player.transform.localScale.x * scaleMultiplier,
            player.transform.localScale.y,
            player.transform.localScale.z
        );
    }
}


using UnityEngine;
using System;

public class BlockController : MonoBehaviour
{
    public int hitsToBreak = 1;
    public int points = 0;
    public event Action OnBlockDestroyed;

    public void Hit()
    {
        hitsToBreak--;
        if (hitsToBreak <= 0)
        {
            OnBlockDestroyed?.Invoke();
            GameManager.Instance.AddPoints(points);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            Hit();
    }
}

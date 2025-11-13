using UnityEngine;
using System;

public class BlockController : MonoBehaviour
{
    [SerializeField]private BlockData data;
    private int _hitsToBreak;
    private int _points;
    private SpriteRenderer spriteRenderer;

    public event Action OnBlockDestroyed;
    // TODO: CREAR UN MANAGER DE EVENTOS SI DA TIEMPO

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.initialSprite;

        _hitsToBreak = data.Hits;
        _points = data.Points;
    }
    public void Hit()
    {
        _hitsToBreak--;
        //Para cambiar el sprite cuando le quede 1 golpe para romperse
        if (_hitsToBreak - 1 == 0)
        {
            spriteRenderer.sprite = data.brokeSprite;
        }

        if (_hitsToBreak <= 0)
        {
            //TODO: EVENTMANAGER
            OnBlockDestroyed?.Invoke();
            GameManager.Instance.AddPoints(_points);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            Hit();
    }
}

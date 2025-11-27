using UnityEngine;
using System;

public class BlockController : MonoBehaviour
{
    [SerializeField]private BlockData data;
    private int _hitsToBreak;
    private int _points;
    private SpriteRenderer _spriteRenderer;
    private bool _isDestroyed = false; // para evitar dobles colisiones


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.initialSprite;

        _hitsToBreak = data.Hits;
        _points = data.Points;
    }
    public void Hit()
    {
        if (_isDestroyed) return; // Si ya está destruido, no hacer nada
        _hitsToBreak--;
        //Para cambiar el sprite cuando le quede 1 golpe para romperse
        if (_hitsToBreak - 1 == 0)
        {
            _spriteRenderer.sprite = data.brokeSprite;
        }

        if (_hitsToBreak <= 0)
        {
            _isDestroyed = true; // Marcamos como destruido
            // Invoco el evento de destruccion de bloque
            EventManager.Instance.InvokeBlockDestroyed();
            // Generamos un power up
            PowerUpManager.Instance.GeneratePowerUp(this.transform);
            GameManager.Instance.AddPoints(_points);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            Hit();
    }
}

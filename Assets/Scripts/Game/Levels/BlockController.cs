using UnityEngine;
using System;

public class BlockController : MonoBehaviour
{
    [SerializeField]private BlockData data;// Datos del bloque
    private int _hitsToBreak;// vida del bloque
    private int _points;// Puntos
    private SpriteRenderer _spriteRenderer;// Sprite del bloque
    private bool _isDestroyed = false; // para evitar dobles colisiones


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.initialSprite;// Asignamos el sprite

        _hitsToBreak = data.Hits;// Recuperamos datos del BlockData
        _points = data.Points;
    }
    public void Hit()
    {
        if (_isDestroyed) return; // Si ya está destruido, no hacer nada
        _hitsToBreak--;
        //Para cambiar el sprite cuando le quede 1 golpe para romperse
        if (_hitsToBreak - 1 == 0)
        {
            _spriteRenderer.sprite = data.brokeSprite;// Cambiamos a sprite roto
        }

        if (_hitsToBreak <= 0)
        {
            _isDestroyed = true; // Marcamos como destruido
            // Invoco el evento de destruccion de bloque
            EventManager.Instance.InvokeBlockDestroyed();
            // Generamos un power up
            PowerUpManager.Instance.GeneratePowerUp(this.transform);// Generamos un powerUp
            GameManager.Instance.AddPoints(_points);// Actualizamos los puntos 
            Destroy(this.gameObject);// Destruimos el bloque
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))// Si colisona con la bola
            Hit();
    }
}

using UnityEngine;
using System;

public class BlockController : MonoBehaviour
{
    [SerializeField]private BlockData data;
    private int _hitsToBreak;
    private int _points;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.initialSprite;

        _hitsToBreak = data.Hits;
        _points = data.Points;
    }
    public void Hit()
    {
        _hitsToBreak--;
        //Para cambiar el sprite cuando le quede 1 golpe para romperse
        if (_hitsToBreak - 1 == 0)
        {
            _spriteRenderer.sprite = data.brokeSprite;
        }

        if (_hitsToBreak <= 0)
        {
           
            // Invoco el evento de destruccion de bloque
            EventManager.Instance.InvokeBlockDestroyed();
            // Generamos un power up
            // TODO: alguna manera para no tener que estar llamando este metodo cuando se hayan generado el numero maximo de powerups del nivel
            // Podria suscribirme desde el POwerUpManager pero necesito la posicion del bloque para instanciar el prefab
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

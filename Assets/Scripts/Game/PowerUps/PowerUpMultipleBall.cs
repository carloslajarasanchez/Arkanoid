using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpMultipleBall : PowerUp
{
    [SerializeField] private GameObject ball;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        stackeable = true;       
    }
    // Añade varias bolas
    public override void ApplyEffect()
    {
        if (ball == null)
        {
            ball = FindObjectOfType<Ball>().gameObject;// Recogemos la referencia de la bola
        }
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = ball.transform.position;// Asignamos la posicion de de la bola
            GameObject newBall = Instantiate(ball, spawnPos, Quaternion.identity);// Instanciamos la bola

            Ball ballScript = newBall.GetComponent<Ball>();
            ballScript.IsClone = true; // evita ResetBall()

            Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
            rb.velocity = randomDir * 11f; // velocidad inicial
        }

        GameManager.Instance.Balls += 3;// Aumentamos la cantidad de bolas

    }
    public override void Remove()
    {

    }
}

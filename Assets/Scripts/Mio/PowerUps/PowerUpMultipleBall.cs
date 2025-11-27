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

    public override void ApplyEffect()
    {
        if (ball == null)
        {
            ball = FindObjectOfType<Ball>().gameObject;
        }
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = ball.transform.position;
            GameObject newBall = Instantiate(ball, spawnPos, Quaternion.identity);

            Ball ballScript = newBall.GetComponent<Ball>();
            ballScript.IsClone = true; // evita ResetBall()

            Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
            rb.velocity = randomDir * 11f; // velocidad inicial
        }

        GameManager.Instance.Balls += 3;

    }

    public override void Remove()
    {

    }
}

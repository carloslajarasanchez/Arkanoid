using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PrimerScript : MonoBehaviour
{
    List<Color> colores = new List<Color>();
    private SpriteRenderer _spriteRenderer;
    int aux = 0;
    [SerializeField] private Sprite _sprite;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        colores.Add(Color.red);
        colores.Add(Color.blue);
        colores.Add(Color.yellow);
        colores.Add(Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _spriteRenderer.color = colores[aux];
            aux++;
            if (aux == 4)
            {
                aux = 0;
            }
            
        }
    }
}

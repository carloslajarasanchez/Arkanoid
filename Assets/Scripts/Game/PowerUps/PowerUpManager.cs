using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [SerializeField] private List<GameObject> _powerUpsPrefabs;// Lista de los prefbs

    [SerializeField] private int _maxQuantity = 10;// Maxima cantidad de powerUp
    [SerializeField] private int _minQuantity = 4;// Minima cantidad de powerUp
    private int _currentlyPowerUps = 0;// Cantidad de PowerUp generadas
    private int _toGeneratePowerUps = 0;// Cantidad de PowerUp para generar

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
    }
    // Metodo para generar PowerUp
    public void GeneratePowerUp(Transform blockPosition)
    {
        Debug.Log("Generando PowerUp");
        int probability = Random.Range(0, 5);// Generamos la probabilidad de que se genere un powerUp

        if(probability == 2 && _currentlyPowerUps < _toGeneratePowerUps)// Si la probabilidad es 2 y los PowerUps generados son menor que los que se deben generar
        {
            int aux = Random.Range(0, _powerUpsPrefabs.Count);// Creamos una variable para seleccionar un prefab aleatorio
            Instantiate(_powerUpsPrefabs[aux], blockPosition.position, _powerUpsPrefabs[aux].transform.rotation);// Generamos el prefab
            Debug.Log("Se ha generado el powerUp");
            _currentlyPowerUps++;// Aumentamos la cantidad de prefabs generados
        }
        
    }

    // Metodo para reiniciar la cantidad de powerUps
    public void ResetPowerUps()
    {
        _currentlyPowerUps = 0;
        _toGeneratePowerUps = Random.Range(_minQuantity, _maxQuantity);
        Debug.Log($"PowerUps a generar {_toGeneratePowerUps}");
    }
}

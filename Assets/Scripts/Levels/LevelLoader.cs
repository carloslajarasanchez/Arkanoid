using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelLoader : MonoBehaviour
{
    [Header("Block Prefabs of type (índice = número en JSON)")]
    [SerializeField] private GameObject[] blockPrefabs;// Prefabs de los bloques a instanciar

    [Header("Level configuration")]
    [SerializeField]private float blockWidth = 1f;// Ancho de los bloques
    [SerializeField]private float blockHeight = 0.5f;// Alto de los bloques
    public Vector2 startPos = new Vector2(-4f, 4f);// Posicion inicial donde se instancian los bloques

    private int _remainingBlocks = 0;// Bloques que quedan en total
    private bool _loadingNext = false;// Para controlar cuando pasar al siguiente nivel
    private string _level = "level";// Variable para controlar el nombre del archivo JSon

    private int _counter = 1;// Contador de niveles
    private int _totalLevels;// Cantidad de niveles

    void Start()
    {
        // Cargar todos los archivos JSON que empiecen por "level"
        TextAsset[] allLevels = Resources.LoadAll<TextAsset>("");

        // Filtrar solo los que contienen "level" en el nombre
        _totalLevels = allLevels.Count(t => t.name.StartsWith("level"));

        Debug.Log($"Se encontraron {_totalLevels} niveles.");

        //Actulizamos el nivel en el que estamos
        SaveLevel();

        string levelname = _level + _counter.ToString();// Cargamos el nivel que toque con el counter concatenando 
        EventManager.Instance.OnGameFinished += SaveLevel;// Para mandar el nivel maximo al que hemos llegado
        EventManager.Instance.OnBallLosted += SaveLevel;// Para detectar en que nivel estamos y actualizarlo en la UI   
        EventManager.Instance.OnBlockDestroyed += HandleBlockDestroyed;// Se suscribe al evento de destruccion de bloque, para detectar si se ha roto un bloque
        EventManager.Instance.OnLevelRestarted += RestartLevel;// Nos suscribimos al evento de resetear nivel
        LoadLevel(levelname); // Cargamos el nivel
    }

    // Metodo para generar los bloques del nivel por un Json dependiendo del archivo
    void LoadLevel(string fileName)
    {
        //-----------GENERATE BLOCKS----------
        // Limpiamos los bloques anteriores por seguridad
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Cargamos los JSON desde Resources
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);// Recogemos el archivo json

        if (jsonFile == null)// Si no se encuentra el Json
        {
            Debug.LogError($"No se encontró el archivo {fileName}.json en la carpeta Resources.");
            return;// Volvemos
        }
        else
        {
            // Mostramos informacion por comprobar
            Debug.Log($"Archivo encontrado: {fileName}.json");
            Debug.Log($"Contenido JSON: {jsonFile.text}");
        }

        // Deserializar JSON
        LevelData data = JsonUtility.FromJson<LevelData>(jsonFile.text);

        // Generar bloques
        _remainingBlocks = 0;
        for (int row = 0; row < data.rows.Count; row++)
        {
            for (int col = 0; col < data.rows[row].cols.Count; col++)
            {
                int blockType = data.rows[row].cols[col];
                if (blockType == 0) continue;

                if (blockType < blockPrefabs.Length && blockPrefabs[blockType] != null)
                {
                    Vector2 pos = new Vector2(
                        startPos.x + col * blockWidth,
                        startPos.y - row * blockHeight
                    );

                    GameObject block = Instantiate(blockPrefabs[blockType], pos, Quaternion.identity, transform);

                    _remainingBlocks++;
                }
                else
                {
                    Debug.LogWarning($"No hay prefab asignado para el bloque tipo {blockType}");
                }
            }
        }

        Debug.Log($"Nivel {fileName} cargado con {_remainingBlocks} bloques.");
        _loadingNext = false;
        //-----------GENERATE BLOCKS----------

        //-----------POWER UPS----------
        PowerUpManager.Instance.ResetPowerUps();
        //-----------POWER UPS----------

    }

    void HandleBlockDestroyed()
    {
        
        _remainingBlocks--;
        Debug.Log($"Bloques restantes = {_remainingBlocks}");
        if (_remainingBlocks <= 0 && !_loadingNext)
        {
            _loadingNext = true;
            Debug.Log("Nivel completado. Cargando siguiente...");
            Invoke(nameof(LoadNextLevel), .5f);
        }
    }

    private void SaveLevel()
    {
        GameManager.Instance.Level = _counter;
    }

    void LoadNextLevel()
    {
        
        _counter++;
        Debug.Log($"Counter: {_counter}");
        Debug.Log($"TotalLevels: {_totalLevels}");
        if (_counter > _totalLevels)
        {
            // TODO: NO FUNCIONA
            Debug.Log("¡Has completado todos los niveles!");
            //TODO: Cargar escena final o de victoria
            SceneManager.LoadScene("WinScene");
            return;
        }
        string levelname = _level + _counter.ToString();

        // Llamamos al evento de nivel terminado.
        EventManager.Instance.InvokeLevelFinished();
        

        LoadLevel( levelname );
    }

    // Nos desuscribimos del evento para evitar errores
    private void OnDestroy()
    {
        //EventManager.Instance.OnBlockDestroyed -= HandleBlockDestroyed;
        EventManager.Instance.OnGameFinished -= SaveLevel;
        EventManager.Instance.OnBallLosted -= SaveLevel;
    }

    private void RestartLevel()// Recarga el nivel 1
    {
        _counter = GameManager.Instance.Level;
        LoadNextLevel();
    }
}

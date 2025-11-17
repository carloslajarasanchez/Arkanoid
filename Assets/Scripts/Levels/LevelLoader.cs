using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelLoader : MonoBehaviour
{
    [Header("Prefabs de bloques por tipo (índice = número en JSON)")]
    [SerializeField] private GameObject[] blockPrefabs;// Prefabs de los bloques a instanciar

    [Header("Configuración del nivel")]
    [SerializeField]private float blockWidth = 1f;// Ancho de los bloques
    [SerializeField]private float blockHeight = 0.5f;// Alto de los bloques
    public Vector2 startPos = new Vector2(-4f, 4f);// Posicion inicial donde se instancian los bloques

    private int _remainingBlocks = 0;
    private bool _loadingNext = false;// Para controlar cuando pasar al siguiente nivel
    private string _level = "level";

    private int _counter = 1;// Contador de niveles
    private int _totalLevels;// Cantidad de niveles

    void Start()
    {
        // Cargar todos los archivos JSON que empiecen por "level"
        TextAsset[] allLevels = Resources.LoadAll<TextAsset>("");

        // Filtrar solo los que contienen "level" en el nombre
        _totalLevels = allLevels.Count(t => t.name.StartsWith("level"));

        Debug.Log($"Se encontraron {_totalLevels} niveles.");

        string levelname = _level + _counter.ToString();
        EventManager.Instance.OnGameFinished += SaveLevel;
        LoadLevel(levelname); // Cargamos el nivel
    }

    void LoadLevel(string fileName)
    {
        //-----------GENERATE BLOCKS----------
        // Limpiar bloques anteriores por seguridad
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Cargar JSON desde Resources
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);// Recogemos el archivo json
        if (jsonFile == null)
        {
            Debug.LogError($"No se encontró el archivo {fileName}.json en la carpeta Resources.");
            return;
        }
        else
        {
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

                    /*BlockController ctrl = block.GetComponent<BlockController>();
                    if (ctrl != null)
                        //ctrl.OnBlockDestroyed += HandleBlockDestroyed;*/

                    _remainingBlocks++;
                }
                else
                {
                    Debug.LogWarning($"No hay prefab asignado para el bloque tipo {blockType}");
                }
            }
        }

        // Se suscribe al evento de destruccion de bloque, para detectar si se ha roto un bloque
        EventManager.Instance.OnBlockDestroyed += HandleBlockDestroyed;

        Debug.Log($"Nivel {fileName} cargado con {_remainingBlocks} bloques.");
        //-----------GENERATE BLOCKS----------

        //-----------POWER UPS----------
        PowerUpManager.Instance.ResetPowerUps();
        //-----------POWER UPS----------
    }

    void HandleBlockDestroyed()
    {
        //TODO: Agregar una clase aparte que haga generacion de powerUps Aleatoriamente con un random (Hay que añadir un array de prefabs de powerUps y elegir aleatoriamente entre la lista y si se va a generar o no)
        Debug.Log("HandleBlockDestroyed");
        _remainingBlocks--;

        if (_remainingBlocks <= 0 && !_loadingNext)
        {
            _loadingNext = true;
            Debug.Log("Nivel completado. Cargando siguiente...");
            Invoke(nameof(LoadNextLevel), 1.5f);
        }
    }

    private void SaveLevel()
    {
        GameManager.Instance.SetLevel(_counter);
    }

    void LoadNextLevel()
    {
        Debug.Log($"Counter: {_counter}");
        _counter++;
        if (_counter > _totalLevels)
        {
            // TODO: NO FUNCIONA
            Debug.Log("¡Has completado todos los niveles!");
            //TODO: Cargar escena final o de victoria
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
        EventManager.Instance.OnBlockDestroyed -= HandleBlockDestroyed;
        EventManager.Instance.OnGameFinished -= SaveLevel;
    }
}

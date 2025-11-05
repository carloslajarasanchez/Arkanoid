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


    private Player _player;
    private Ball _ball;

    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _ball = FindAnyObjectByType<Ball>();

        // Cargar todos los archivos JSON que empiecen por "level"
        TextAsset[] allLevels = Resources.LoadAll<TextAsset>("");

        // Filtrar solo los que contienen "level" en el nombre
        _totalLevels = allLevels.Count(t => t.name.StartsWith("level"));

        Debug.Log($"Se encontraron {_totalLevels} niveles.");

        string levelname = _level + _counter.ToString();
        LoadLevel(levelname); // Cargamos el nivel
    }

    void LoadLevel(string fileName)
    {
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

                    BlockController ctrl = block.GetComponent<BlockController>();
                    if (ctrl != null)
                        ctrl.OnBlockDestroyed += HandleBlockDestroyed;

                    _remainingBlocks++;
                }
                else
                {
                    Debug.LogWarning($"No hay prefab asignado para el bloque tipo {blockType}");
                }
            }
        }

        Debug.Log($"Nivel {fileName} cargado con {_remainingBlocks} bloques.");
    }

    void HandleBlockDestroyed()
    {
        Debug.Log("HandleBlockDestroyed");
        _remainingBlocks--;

        if (_remainingBlocks <= 0 && !_loadingNext)
        {
            _loadingNext = true;
            Debug.Log("Nivel completado. Cargando siguiente...");
            Invoke(nameof(LoadNextLevel), 1.5f);
        }
    }

    void LoadNextLevel()
    {
        _counter++;
        if (_counter > _totalLevels)
        {
            Debug.Log("¡Has completado todos los niveles!");
            //TODO: Cargar escena final o de victoria
            return;
        }
        string levelname = _level + _counter.ToString();

        _player.ResetToInitialPosition();// Reiniciamos posicion del player
        _ball.ResetBall();// Reiniciamos posicion de la bola

        LoadLevel( levelname );
    }
}

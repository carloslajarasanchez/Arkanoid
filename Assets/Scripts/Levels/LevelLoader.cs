using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelLoader : MonoBehaviour
{
    [Header("Prefabs de bloques por tipo (índice = número en JSON)")]
    public GameObject[] blockPrefabs;

    [Header("Configuración del nivel")]
    public float blockWidth = 1f;
    public float blockHeight = 0.5f;
    public Vector2 startPos = new Vector2(-4f, 4f);

    private int remainingBlocks = 0;
    private bool loadingNext = false;

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        LoadLevel(currentScene.ToLower()); // ejemplo: "Level1" -> "level1.json"
    }

    void LoadLevel(string fileName)
    {
        // Limpiar bloques anteriores por seguridad
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Cargar JSON desde Resources
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile == null)
        {
            Debug.LogError($"No se encontró el archivo {fileName}.json en la carpeta Resources.");
            return;
        }

        // Deserializar JSON
        LevelData data = JsonUtility.FromJson<LevelData>(jsonFile.text);

        // Generar bloques
        remainingBlocks = 0;
        for (int row = 0; row < data.rows.Length; row++)
        {
            for (int col = 0; col < data.rows[row].Length; col++)
            {
                int blockType = data.rows[row][col];
                if (blockType == 0) continue; // 0 = vacío

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

                    remainingBlocks++;
                }
                else
                {
                    Debug.LogWarning($"No hay prefab asignado para el bloque tipo {blockType}");
                }
            }
        }

        Debug.Log($"Nivel {fileName} cargado con {remainingBlocks} bloques.");
    }

    void HandleBlockDestroyed()
    {
        remainingBlocks--;

        if (remainingBlocks <= 0 && !loadingNext)
        {
            loadingNext = true;
            Debug.Log("Nivel completado. Cargando siguiente...");
            Invoke(nameof(LoadNextScene), 1.5f);
        }
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No hay más niveles. ¡Has completado el juego!");
            // Aquí puedes cargar una escena de victoria o volver al menú:
            // SceneManager.LoadScene(0);
        }
    }
}

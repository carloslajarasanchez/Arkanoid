using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private int _maxEntries = 5;// Numero maximo de rankings

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

    // Metodo que carga los datos del ranking
    public List<ScoreData> LoadRanking()
    {
        List<ScoreData> ranking = new List<ScoreData>();// Se crea una Lista de ScoreData para almacenar los datos

        for (int i = 0; i < _maxEntries; i++)// Se recorre el maximo de entradas
        {
            int level = PlayerPrefs.GetInt("Rank" + i + "_Level", -1);// Recogemos el valor del nivel
            float time = PlayerPrefs.GetFloat("Rank" + i + "_Time", -1f);// Recogemos el valor del tiempo
            int score = PlayerPrefs.GetInt("Rank" + i + "_Score", -1);// Recogemos el valor de los puntos

            if (score >= 0)// Si la puntuacion es mayor que 0
                ranking.Add(new ScoreData(level, time, score));// Se guarda en la lista
        }

        return ranking;// Devuelve la lista
    }
    // Metodo que guarda un ranking
    void SaveRanking(List<ScoreData> ranking)
    {
        for (int i = 0; i < ranking.Count; i++)// Recorre los rankings
        {
            PlayerPrefs.SetInt("Rank" + i + "_Level", ranking[i].Level);// Guardamos el valor del nivel
            PlayerPrefs.SetFloat("Rank" + i + "_Time", ranking[i].Time);// Guardamos el valor del tiempo
            PlayerPrefs.SetInt("Rank" + i + "_Score", ranking[i].Score);// Guardamos el valor del score
        }

        PlayerPrefs.Save();// Se guardan los playerprefs
    }

    // Metodo que añade un nuevo ranking
    public void AddNewScore(int level, float time, int score)
    {
        List<ScoreData> ranking = LoadRanking();// Creamos una lista de Rankings

        ranking.Add(new ScoreData(level, time, score));// Añadimos la puntuacion

        // Ordenar por puntuación descendente (puedes cambiar criterio)
        ranking = ranking.OrderByDescending(x => x.Score).ToList();

        // Mantener solamente top N
        if (ranking.Count > _maxEntries)
            ranking = ranking.Take(_maxEntries).ToList();

        SaveRanking(ranking);// Guardamos el ranking
    }
}

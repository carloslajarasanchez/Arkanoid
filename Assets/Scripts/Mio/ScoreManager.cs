using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private int _maxEntries = 5;

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

    public List<ScoreData> LoadRanking()
    {
        List<ScoreData> ranking = new List<ScoreData>();

        for (int i = 0; i < _maxEntries; i++)
        {
            int level = PlayerPrefs.GetInt("Rank" + i + "_Level", -1);
            float time = PlayerPrefs.GetFloat("Rank" + i + "_Time", -1f);
            int score = PlayerPrefs.GetInt("Rank" + i + "_Score", -1);

            if (score >= 0)
                ranking.Add(new ScoreData(level, time, score));
        }

        return ranking;
    }

    void SaveRanking(List<ScoreData> ranking)
    {
        for (int i = 0; i < ranking.Count; i++)
        {
            PlayerPrefs.SetInt("Rank" + i + "_Level", ranking[i].Level);
            PlayerPrefs.SetFloat("Rank" + i + "_Time", ranking[i].Time);
            PlayerPrefs.SetInt("Rank" + i + "_Score", ranking[i].Score);
        }

        PlayerPrefs.Save();
    }

    public void AddNewScore(int level, float time, int score)
    {
        List<ScoreData> ranking = LoadRanking();

        ranking.Add(new ScoreData(level, time, score));

        // Ordenar por puntuación descendente (puedes cambiar criterio)
        ranking = ranking.OrderByDescending(x => x.Score).ToList();

        // Mantener solamente top N
        if (ranking.Count > _maxEntries)
            ranking = ranking.Take(_maxEntries).ToList();

        SaveRanking(ranking);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData // Clase serializable para guardar los valores de los ranking
{
    public int Level;
    public float Time;
    public int Score;

    // Constructor de los datos
    public ScoreData(int level, float time, int score)
    {
        this.Level = level;
        this.Time = time;
        this.Score = score;
    }
}

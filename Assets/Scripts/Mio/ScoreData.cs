using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData 
{
    public int Level;
    public float Time;
    public int Score;

    public ScoreData(int level, float time, int score)
    {
        this.Level = level;
        this.Time = time;
        this.Score = score;
    }
}

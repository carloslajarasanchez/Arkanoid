using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelRow
{
    public List<int> cols;
}

[System.Serializable]
public class LevelData
{
    public List<LevelRow> rows;
}

using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelRow
{
    public System.Collections.Generic.List<int> cols;
}

[System.Serializable]
public class LevelData
{
    public System.Collections.Generic.List<LevelRow> rows;
}

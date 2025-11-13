using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName ="ScriptableObjet/Create BlockData", order = 1)]
public class BlockData : ScriptableObject
{
    //Estos scripts no tienen logica dentro
    //Sirven para dar datos para objetos
    public int Hits;
    public int Points;
    public Sprite initialSprite;
    public Sprite brokeSprite;

}

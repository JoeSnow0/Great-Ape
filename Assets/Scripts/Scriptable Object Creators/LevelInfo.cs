using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Levels/Level Info", order = 1)]
public class LevelInfo : ScriptableObject
{
    // Ingame display name
    public string levelName;
    public Sprite thumbnail;
    [HideInInspector] public int world;
    [Range(0, 4)] public int score;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Levels/Level Info", order = 1)]
public class LevelInfo : ScriptableObject
{
    public string levelName;
    public Sprite thumbnail;
    public Object scene;
    public int world;
    [Range(0, 3)]public int score;
}
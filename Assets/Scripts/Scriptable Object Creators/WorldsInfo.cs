using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldsInfo", menuName = "Levels/Worlds Info", order = 1)]
public class WorldsInfo : ScriptableObject{
    public List<string> worldNames = new List<string>();
}

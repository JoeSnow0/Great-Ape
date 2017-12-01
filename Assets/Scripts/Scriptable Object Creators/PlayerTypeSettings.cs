using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerTypeSettings : MonoBehaviour
{
    
    [Header("Jump")]
    //public abilityName;
    [Header("Name of Keybinding")]
    public string name;
}
//Creates a scriptable object with the parameters from PlayerTypeSettings
[CreateAssetMenu(fileName = "NameTypeStats", menuName = "PlayerCharacters/CreateNewTypeStats", order = 1)]
public class AbilityInfoObject : ScriptableObject
{
    //HACK: Not sure if this works
    public PlayerTypeSettings PlayerTypeStats = new PlayerTypeSettings();
}

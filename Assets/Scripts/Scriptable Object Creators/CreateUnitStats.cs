using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

//Creates a scriptable object with the parameters from PlayerTypeSettings
[CreateAssetMenu(fileName = "UnitNameStats", menuName = "PlayerCharacters/CreateNewUnitStats", order = 1)]
public class CreateUnitStats : ScriptableObject
{
    public enum SpecialAction { Throw, Sprint, Punch };

    [Header("Jump")]
    [ Range(1, 3)]
    public int maxJumpHeight;
    [Header("Movement")]
    [Range(1, 3)]
    public int moveSpeed;
    [Range(1, 3)]
    public int weight;
    [Header("Special Action")]
    public SpecialAction assignedAction;
    [Range(1,3)]
    public int computerSkills;
    [Header("Sprite")]
    public Sprite unitSprite;
    public Animation unitAnimation;


}

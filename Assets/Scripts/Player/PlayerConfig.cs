using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    //A collection of all scripts for the player character
    public PlayerController playerController;
    public PlayerGraphics playerGraphics;
    public bool isActive;
    public CreateUnitStats unitStats;

    //Add failsafes here
    private void Start()
    {
    }

    public void ToggleApe()
    {
        isActive = !isActive;
    }
}


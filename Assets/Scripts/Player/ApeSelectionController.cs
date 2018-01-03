﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApeSelectionController : MonoBehaviour
{
    public ManagerConfig managerConfig;
    public List<Player> apeList = new List<Player>();
    static public Player activeApe;
    public Player[] apePrefabs;
    public GameObject arrowObject;

    private void Start()
    {
        InitializeApes();
    }
    private void Update()
    {
        //For testing
        if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchApe(activeApe, true);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchApe(activeApe, false);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            AddApe(0, managerConfig.apeStart.transform.position, managerConfig.apeStart.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            AddApe(1, managerConfig.apeStart.transform.position, managerConfig.apeStart.transform.rotation);
        }
    }
    public void InitializeApes()
    {
        //Update list of apes, add all available apes in the scene
        AddApe(0, managerConfig.apeStart.transform.position, managerConfig.apeStart.transform.rotation);
        //Target first ape in list
        apeList[0].playerInput.SetApeState(true);
        activeApe = apeList[0];
        DeselectAllOtherApes();
        //Assign Camera to active ape
        managerConfig.mainCamera.SetTarget(activeApe.controller);
        MoveArrowToApe();
    }
    public void AddApe(int apeType, Vector3 SpawnPosition, Quaternion spawnRotation)
    {
        //Instansiate new ape, assign stats, animation etc
        Player newApe = Instantiate(apePrefabs[apeType], SpawnPosition, spawnRotation, managerConfig.apeHolder.transform);
        //Add it to the ape list
        apeList.Add(newApe);
    }

    // Switches ape to the next or previous depending on the bool "next"
    public void SwitchApe(Player apeTargetOld, bool next)
    {
        if(!apeTargetOld.playerInput.GetApeState())
        {
            return;
        }
        //Select the next or previous ape in the list
        int tempIndex = apeList.IndexOf(apeTargetOld) + ((next)?1:-1);
        tempIndex = (tempIndex < 0) ? apeList.Count - 1 : tempIndex;

        tempIndex = tempIndex % apeList.Count;
        activeApe = apeList[tempIndex];
        managerConfig.mainCamera.SetTarget(activeApe.controller);
        DeselectAllOtherApes();
        MoveArrowToApe();
        //Activate ape
        activeApe.playerInput.SetApeState(true);
    }

    public void DeselectAllOtherApes()
    {
        foreach (Player ape in apeList)
        {
            ape.playerInput.SetApeState(false);
        }
        activeApe.playerInput.SetApeState(true);
    }
    public void DeselectAllApes()
    {
        foreach (Player ape in apeList)
        {
            ape.playerInput.SetApeState(false);
        }
    }
    // Moves the indicator to be slightly above the current ape
    private void MoveArrowToApe()
    {
        arrowObject.transform.SetParent(activeApe.transform);
        arrowObject.transform.localPosition = Vector3.up * 4;
    }
}

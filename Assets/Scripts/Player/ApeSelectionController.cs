﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Anima2D;

public class ApeSelectionController : MonoBehaviour
{
    public ManagerConfig managerConfig;
    public List<Player> apeList = new List<Player>();
    static public Player activeApe;
    public Player[] apePrefabs;
    public GameObject arrowObject;
    static int apeCount = 0;

    private void Start()
    {
        InitializeApes();
    }
    private void Update()
    {
        //For cheats
        /*
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            AddApe(apePrefabs[0], managerConfig.apeStart.transform.position, managerConfig.apeStart.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            AddApe(apePrefabs[1], managerConfig.apeStart.transform.position, managerConfig.apeStart.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            AddApe(apePrefabs[2], managerConfig.apeStart.transform.position, managerConfig.apeStart.transform.rotation);
        }
        */

        //previous ape
        if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            SwitchApe(activeApe, true);
        }
        //Next ape
        else if (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchApe(activeApe, false);
        }
    }
    public void InitializeApes()
    {
        //Update list of apes, add all available apes in the scene
        AddApe(apePrefabs[0], managerConfig.apeStart.transform.position, managerConfig.apeStart.transform.rotation);
        //Target first ape in list
        apeList[0].playerInput.SetApeState(true);
        activeApe = apeList[0];
        DeselectAllOtherApes();
        //Assign Camera to active ape
        managerConfig.mainCamera.SetTarget(activeApe.controller);
        apeList[0].managerConfig = managerConfig;
        MoveArrowToApe();
    }
    public void AddApe(Player apeType, Vector3 SpawnPosition, Quaternion spawnRotation)
    {
        //Instansiate new ape, assign stats, animation etc
        Player newApe = Instantiate(apeType, SpawnPosition, spawnRotation, managerConfig.apeHolder.transform);

        Gradient colRange = newApe.colorRange;
        Color newApeCol = colRange.Evaluate(Random.value);
        foreach (SpriteMeshInstance smi in newApe.GetComponentsInChildren<SpriteMeshInstance>())
        {
            smi.sortingOrder = ++apeCount;
            smi.color = newApeCol;
        }
        //Add it to the ape list
        apeList.Add(newApe);
    }
    public void RemoveApe(Player ape)
    {
        //Remove ape from list
        apeList.Remove(ape);
        //Delete it from scene
        Destroy(ape.gameObject);
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

        //Activate any adjacent lever's triggers
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
        arrowObject.transform.localPosition = Vector3.up * 5;
    }

    public  void RemoveAllApes()
    {
        activeApe = null;
        foreach(Player ape in apeList)
        {
            Destroy(ape.gameObject);
        }
        apeList.Clear();
    }
}

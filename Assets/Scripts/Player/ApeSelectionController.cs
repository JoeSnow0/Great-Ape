using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApeSelectionController : MonoBehaviour
{
    public ManagerConfig managerConfig;
    public List<PlayerConfig> apeList = new List<PlayerConfig>();
    static public PlayerConfig activeApe;
    public PlayerConfig[] apePrefabs;
    public GameObject spawnPoint;

    public GameObject arrowObject;
    //public PlayerConfig[] apeList;
    private void Start()
    {
        InitializeApes();
    }
    private void Update()
    {
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
            AddApe(0, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            AddApe(1, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
    public void InitializeApes()
    {
        //Update list of apes, add all available apes in the scene
        AddApe(0, spawnPoint.transform.position, spawnPoint.transform.rotation);
        //Target first ape in list
        apeList[0].isActive = true;
        activeApe = apeList[0];
        DeselectAllOtherApes();
        MoveArrowToApe();
    }
    public void AddApe(int apeType, Vector3 SpawnPosition, Quaternion spawnRotation)
    {
        //Instansiate new ape, assign stats, animation etc
        PlayerConfig newApe = Instantiate<PlayerConfig>(apePrefabs[apeType], SpawnPosition, spawnRotation, managerConfig.apeHolder.transform);
        //disable ape
        newApe.isActive = false;
        //Add it to the ape list
        apeList.Add(newApe);
    }

    // Switches ape to the next or previous depending on the bool "next"
    public void SwitchApe(PlayerConfig apeTargetOld, bool next)
    {
        //Select the next or previous ape in the list
        int tempIndex = apeList.IndexOf(apeTargetOld) + ((next)?1:-1);
        tempIndex = (tempIndex < 0) ? apeList.Count - 1 : tempIndex;

        tempIndex = tempIndex % apeList.Count;
        activeApe = apeList[tempIndex];
        DeselectAllOtherApes();
        MoveArrowToApe();
        print(tempIndex);
    }


    public void DeselectAllOtherApes()
    {
        foreach (PlayerConfig ape in apeList)
        {
            ape.isActive = false;
        }
        activeApe.isActive = true;
    }

    // Moves the indicator to be slightly above the current ape
    private void MoveArrowToApe()
    {
        arrowObject.transform.SetParent(activeApe.transform);
        arrowObject.transform.localPosition = Vector3.up * 4;
    }
}

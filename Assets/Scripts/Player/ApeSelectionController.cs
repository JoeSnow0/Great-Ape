using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApeSelectionController : MonoBehaviour
{
    public ManagerConfig managerConfig;
    public List<PlayerConfig> apeList = new List<PlayerConfig>();
    static public PlayerConfig activeApe;
    public PlayerConfig[] apePrefabs;
    //public PlayerConfig[] apeList;
    private void Start()
    {
        InitializeApes();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            SelectNextApe(activeApe);
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            SelectPreviousApe(activeApe);
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            AddApe(0, managerConfig.apeHolder.transform.position, managerConfig.apeHolder.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            AddApe(1, managerConfig.apeHolder.transform.position, managerConfig.apeHolder.transform.rotation);
        }
    }
    public void InitializeApes()
    {
        //Update list of apes, add all available apes in the scene
        AddApe(0, managerConfig.apeHolder.transform.position, managerConfig.apeHolder.transform.rotation);
        //Target first ape in list
        apeList[0].isActive = true;
        activeApe = apeList[0];
        DeselectAllOtherApes();
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
    public void SelectNextApe(PlayerConfig apeTargetOld)
    {
        //Select the next ape in the list
        int tempIndex = apeList.IndexOf(apeTargetOld) + 1;


        tempIndex = tempIndex % apeList.Count;
        activeApe = apeList[tempIndex];
        DeselectAllOtherApes();
        print(tempIndex);
    }
    public void SelectPreviousApe(PlayerConfig apeTargetOld)
    {
        //Select the previous ape in the list
        int tempIndex = apeList.IndexOf(apeTargetOld) - 1;
        if(tempIndex < 0)
        {
            tempIndex = apeList.Count - 1;
        }
        tempIndex = tempIndex % apeList.Count;
        activeApe = apeList[tempIndex];
        DeselectAllOtherApes();
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
}

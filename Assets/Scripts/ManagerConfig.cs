using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerConfig : MonoBehaviour {

    //Manager has references to everything in the scene
    //Its essencially god, worship it!
    public ApeSelectionController apeSelectionController;
    public GameObject apeHolder;
    public GameObject apeStart;
    public GoalTrigger apeGoal;
    public GameObject ingameMenu;
    public CameraFollow mainCamera;
    public GameObject GameEnd;
    public GameObject Background;
    public GameObject Mainground;
    public GameObject Foreground;
}

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
    public SoundManager soundManagerPrefab;
    public MusicManager musicManagerPrefab;
    public SoundManager soundManager;
    public MusicManager musicManager;

    private void Awake()
    {
        soundManager = (SoundManager)FindObjectOfType(typeof(SoundManager));
        if (soundManager)
        {
            Debug.Log("soundManager found, nothing changed");
        }
            
        else
        {
            soundManager = Instantiate(soundManagerPrefab, transform.position, transform.rotation);
            Debug.Log("soundManager NOT found, adding new prefab");
        }

        musicManager = (MusicManager)FindObjectOfType(typeof(MusicManager));
        if (musicManager)
        {
            Debug.Log("musicManager found, nothing changed");
        }

        else
        {
            musicManager = Instantiate(musicManagerPrefab, transform.position, transform.rotation);
            Debug.Log("musicManager NOT found, adding new prefab");
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] Text playText;
    [SerializeField] Transform worldsParent;
    [SerializeField] Transform nameParent;
    [SerializeField] GameObject nameObject;
    [SerializeField] GameObject levelsObject;
    [SerializeField] GameObject levelObject;
    [SerializeField] RectTransform scrollContent;
    [SerializeField] WorldsInfo worldsInfo;
    //[SerializeField] Animator transitionOverlay;
    InterfaceAudio interfaceAudio;


    static public LevelInfo[] levels;
    List<Transform> worlds = new List<Transform>();

    [SerializeField] Color[] colors;

    static public int lastIndexWithScore;
    List<string> worldNames = new List<string>();

    private int levelLoadInt;
    private string levelLoadString;
    private bool loadLevel = false;
    private bool isInt = true;
    private float timestamp;
    private float currentTime;


    void Start () {
        interfaceAudio = GetComponent<InterfaceAudio>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener(delegate { interfaceAudio.PlayAudioClip(0); });


        worldNames = worldsInfo.worldNames;

        
        levels = Resources.LoadAll<LevelInfo>("Levels");

        for (int i = 0; i < worldNames.Count; i++)// Create the worlds
        {
            worlds.Add(Instantiate(levelsObject, worldsParent).transform);
            Instantiate(nameObject, nameParent).GetComponent<Text>().text = worldNames[i];
        }

        for (int i = 0; i < levels.Length; i++)// Add the levels the the worlds
        {
            int world;
            if(levels[i].world > worlds.Count - 1)
            {
                world = worlds.Count - 1;
            }
            else
            {
                world = levels[i].world;
            }
            Instantiate(levelObject, worlds[world]).GetComponent<LevelButton>().
                Initialize(levels[i].levelName, 
                levels[i].thumbnail, 
                levels[(i > 0) ? i - 1 : 0].score, 
                levels[i].score, colors[0], 
                levels[i].scene.name,
                this,
                interfaceAudio, 
                entry);
        }

        lastIndexWithScore = GetLast();
        
        if(playText != null)
            playText.text = (lastIndexWithScore > 0) ? "Continue" : "Play";// Set play button text

        scrollContent.sizeDelta = new Vector2(scrollContent.rect.width, (worlds.Count * 100) * 1.3f);
	}

    private void Update()
    {
        if (loadLevel)
        {
            if(currentTime >= timestamp)
            {
                if (isInt)
                {
                    SceneManager.LoadScene(levels[levelLoadInt].scene.name);
                }
                else
                {
                    SceneManager.LoadScene(levelLoadString);
                }
                if (Transition.animator != null)
                {
                    Transition.animator.SetBool("fade", false);
                }
                else
                {
                    Debug.Log("Loading completed");
                }
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }
    }

    public void LoadScene(string scene)
    {
        loadLevel = true;
        levelLoadString = scene;
        isInt = false;
        currentTime = Time.time;
        timestamp = Time.time + 1;
        if (Transition.animator != null)
        {
            Transition.animator.SetBool("fade", true);
        }
        else
        {
            Debug.Log("Loading level");
        }
        //SceneManager.LoadScene(scene);
    }
    public void LoadScene(int scene)
    {
        loadLevel = true;
        levelLoadInt = scene;
        isInt = true;
        currentTime = Time.time;
        timestamp = Time.time + 1;
        Transition.animator.SetBool("fade", true);
        //SceneManager.LoadScene(levels[scene].scene.name);
    }

    public int GetLast()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].score <= 0)
            {
                return i;
            }
            else if (i >= levels.Length - 1)
                return -1;
        }
        return 1;
    }
}

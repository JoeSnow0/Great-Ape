using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelect : MonoBehaviour {
    //static public List<KeyValuePair<int, LevelInfo>> levels = new List<KeyValuePair<int, LevelInfo>>();
    [Header("Objects")]
    [SerializeField] Text playText;
    [SerializeField] Transform worldsParent;
    [SerializeField] Transform nameParent;
    [SerializeField] GameObject nameObject;
    [SerializeField] GameObject levelsObject;
    [SerializeField] GameObject levelObject;
    [SerializeField] RectTransform scrollContent;


    static public LevelInfo[] levels;
    List<Transform> worlds = new List<Transform>();

    [SerializeField] Color[] colors;
    [SerializeField] string[] worldNames;

    static public int lastIndexWithScore;

    void Start () {
        levels = Resources.LoadAll<LevelInfo>("Levels");

        for (int i = 0; i < worldNames.Length; i++)// Create the worlds
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
                levels[i].scene.name);
        }

        lastIndexWithScore = GetLast();

        playText.text = (lastIndexWithScore > 0) ? "Continue" : "Play";// Set play button text

        scrollContent.sizeDelta = new Vector2(scrollContent.rect.width, (worlds.Count * 100) * 1.3333f);
	}
	
    static public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    static public void LoadScene(int scene)
    {
        SceneManager.LoadScene(levels[scene].scene.name);
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

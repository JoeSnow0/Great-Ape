using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class LevelBlockSelect : MonoBehaviour
{
    private static LevelBlockSelect currentBlockSelect;
    public static LevelBlockSelect current
    {
        get { return currentBlockSelect; }
    }

    [SerializeField]
    private GameObject buttonHolder;

    [SerializeField]
    private GameObject buttonPrefab;

    private string m_currentBlockPath;

    private void Awake()
    {
        if (currentBlockSelect == null)
            currentBlockSelect = this;
    }

    // Generates a list of buttons for all the level blocks
    public void GenerateButtons()
    {
        foreach(KeyValuePair<string, GameObject> levelBlock in LevelEditor.current.GetLevelBlocks())
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonHolder.transform);

            buttonObj.GetComponent<Image>().sprite = levelBlock.Value.GetComponent<Image>().sprite;

            // Creates an EventTrigger for when you press the button and adds the trigger to it
            EventTrigger trigger = buttonObj.GetComponent<Button>().GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            // Makes the button press call the method PickCurrentBlock
            entry.callback.AddListener((eventData) => { PickCurrentBlock(levelBlock.Key); });
        }
    }

    // Sets the current level block prefab path based on which button the user pressed
    void PickCurrentBlock(string path)
    {
        m_currentBlockPath = path;
    }


    // Returns the currently selected block's prefab path
    public string GetCurrentBlock()
    {
        return m_currentBlockPath;
    }
}

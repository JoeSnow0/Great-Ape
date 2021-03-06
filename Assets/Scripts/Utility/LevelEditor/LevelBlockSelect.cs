﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelBlockSelect : MonoBehaviour
{
    private static LevelBlockSelect m_currentBlockSelect;
    public static LevelBlockSelect current
    {
        get { return m_currentBlockSelect; }
    }

    [SerializeField]
    private InputField m_searchBar;

    [SerializeField]
    private GameObject m_buttonHolder;
    private Dictionary<string, Button> m_buttons;
    private RectTransform m_buttonHolderRT;

    [SerializeField]
    private GameObject m_buttonPrefab;

    [SerializeField]
    private EventSystem m_eventSystem;

    private string m_currentBlockPath;

    private void Awake()
    {
        if (m_currentBlockSelect == null)
            m_currentBlockSelect = this;
    }


    private void Start()
    {
        GenerateButtons();
    }

    private void Update()
    {

    }

    // Generates a list of buttons for all the level blocks
    public void GenerateButtons()
    {
        // Gets all the prefabs in the folder at "Resources/Level Blocks"
        Object[] blockPrefabs = Resources.LoadAll<GameObject>("Level Blocks");
        Texture2D[] previewTextures = Resources.LoadAll<Texture2D>("Level Block Previews") as Texture2D[];

        m_buttons = new Dictionary<string, Button>();

        for (int i = 0; i < blockPrefabs.Length; i++)
        {
            // Creates a button from a prefab
            GameObject buttonObj = Instantiate(m_buttonPrefab, m_buttonHolder.transform);
            buttonObj.name = blockPrefabs[i].name + "_btn";
            buttonObj.transform.GetComponentInChildren<Text>().text = blockPrefabs[i].name;

            // Loads a preview sprite of the object that is saved in the Resources folder
            buttonObj.GetComponent<Image>().sprite = Sprite.Create(previewTextures[i], new Rect(0, 0, previewTextures[i].width, previewTextures[i].height), Vector2.zero);

            // Adds a lambda function that sets the current path chosen
            string path = blockPrefabs[i].name;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SetCurrentBlock(path));
            m_buttons.Add(path, buttonObj.GetComponent<Button>());
        }
        m_buttonHolderRT = m_buttonHolder.GetComponent<RectTransform>();
        UpdateActiveChildCount();
    }

    // Sets the currently selected block
    private void SetCurrentBlock(string path)
    {
        // Gets the button we pressed from our Dictionary of buttons
        Button pressedButton = m_buttons[path];

        // If you press the button for the block already selected, it gets deselected
        if (m_currentBlockPath == path)
        {
            // If the button was highlighted, it is now not highlighted 
            if (m_eventSystem.currentSelectedGameObject == pressedButton.gameObject)
                m_eventSystem.SetSelectedGameObject(null);

            // The backdrop image is deactivated on the button
            pressedButton.transform.GetChild(1).gameObject.SetActive(false);

            // The selection is set to null
            m_currentBlockPath = null;
            return;
        }
        
        // Deselects the other selected object
        if(m_currentBlockPath != null && m_currentBlockPath.Length > 0)
        {
            Button oldButton = m_buttons[m_currentBlockPath];
            oldButton.transform.GetChild(1).gameObject.SetActive(false);
        }

        m_currentBlockPath = path;

        // The backdrop image is activated on the button
        pressedButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    // Sets the size of the button holder to be based on the amount of active children
    public void UpdateActiveChildCount()
    {
        float sizeY = m_buttonHolder.transform.GetActiveChildCount() * 300;
        sizeY = Mathf.Clamp(sizeY, 400, Mathf.Infinity);
        m_buttonHolderRT.sizeDelta = new Vector2(m_buttonHolderRT.sizeDelta.x, sizeY);
    }

    // Returns the currently selected block's prefab path
    public string GetCurrentBlock()
    {
        return m_currentBlockPath;
    }

    // Updates the block select based on the search bar
    public void SearchBlocks()
    {
        string search = m_searchBar.text.ToLower();

        foreach(KeyValuePair<string, Button> buttonPair in m_buttons)
        {
            // Sets the button to be active if it's name contains some of the search string
            string buttonName = buttonPair.Value.name;
            buttonPair.Value.gameObject.SetActive(buttonName.Remove(buttonName.Length - 4, 4).ToLower().Contains(search));
        }
        // Updates the active child count
        UpdateActiveChildCount();
    }
}

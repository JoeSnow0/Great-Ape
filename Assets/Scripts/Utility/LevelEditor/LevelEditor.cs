using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    private static LevelEditor currentEditor;
    public static LevelEditor current
    {
        get { return currentEditor; }
    }

    [ReadOnly]
    private Dictionary<string, GameObject> m_levelBlocks = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if(currentEditor == null)
            currentEditor = this;

        // Gets all the available level blocks
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            m_levelBlocks.Add(child.name, child);
        }
    }

    private void Start()
    {
        LevelBlockSelect.current.GenerateButtons();
    }

    public Dictionary<string, GameObject> GetLevelBlocks()
    {
        return m_levelBlocks;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

// Custom Inspector for the Level Editor that adds a button to update the available level blocks
[CustomEditor(typeof(LevelEditor))]
public class LevelEditorInspector : Editor
{
    LevelEditor levelEditor;

    public override void OnInspectorGUI()
    {
        levelEditor = (LevelEditor)target; 

        DrawDefaultInspector();

        if (GUILayout.Button("Update Available Level Blocks"))
        {
            UpdateLevelBlocks();
        }

        if (GUILayout.Button("Remove Existing Level Blocks"))
        {
            RemoveLevelBlocks();
        }
    }

    // Updates the available objects in the level editor
    private void UpdateLevelBlocks()
    {
        Dictionary<string, GameObject> levelBlocks = new Dictionary<string, GameObject>();

        // Gets all the prefabs in the folder at "Resources/Level Blocks"
        Object[] blockPrefabs = Resources.LoadAll<GameObject>("Level Blocks");
        string[] childNames = GetChildNames();
        foreach (Object block in blockPrefabs)
        {
            // If the object already exists among the available ones we'll skip it
            if (childNames.Contains(block.name))
                continue;
            // Reimports asset so it's updated
            AssetDatabase.ImportAsset("Assets/Resources/Level Blocks/" + block.name + ".prefab");

            // Creates an instance of the level block so it's accessible in the level editor
            GameObject newBlock = Instantiate(block, levelEditor.transform) as GameObject;
            newBlock.name = block.name;

            // Adds a preview image to the object which will be used in the block select later
            Texture2D previewTex = AssetPreview.GetAssetPreview(block);
            Sprite previewSpr = Sprite.Create(previewTex, new Rect(0, 0, previewTex.width, previewTex.height), Vector2.zero);
            newBlock.AddComponent<Image>().sprite = previewSpr;

            //newBlock.hideFlags = HideFlags.HideInHierarchy;
            levelBlocks.Add(block.name, newBlock);
        }
        AssetDatabase.Refresh();
    }

    // Removes the existing objects
    private void RemoveLevelBlocks()
    {
        for (int i = 0; i < levelEditor.transform.childCount;)
        {
            GameObject child = levelEditor.transform.GetChild(i).gameObject;
            DestroyImmediate(child.GetComponent<Image>());
            DestroyImmediate(child);
        }
    }

    private string[] GetChildNames()
    {
        string[] names = new string[levelEditor.transform.childCount];
        for(int i = 0; i < levelEditor.transform.childCount; i++)
        {
            names[i] = levelEditor.transform.GetChild(i).name;
        }

        return names;
    }
}

/////////////////////
///
/// Authored by: Oskar Svensson (Dec 05, 2017)
/// 
/// oskar0svensson@gmail.com
/// 
////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEditor;

// Custom Inspector for the Level Editor that adds a button to update the available level blocks
[CustomEditor(typeof(LevelEditor))]
public class LevelEditorInspector : Editor
{
    //LevelEditor levelEditor;

    public override void OnInspectorGUI()
    {
        //levelEditor = (LevelEditor)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Update Available Level Blocks"))
        {
            UpdateLevelBlocks();
        }
    }

    // Updates the available objects in the level editor
    private void UpdateLevelBlocks()
    {
        // Gets all the prefabs in the folder at "Resources/Level Blocks"
        Object[] blockPrefabs = Resources.LoadAll<GameObject>("Level Blocks");

        foreach (Object block in blockPrefabs)
        {
            // Reimports asset so it's updated
            AssetDatabase.ImportAsset("Assets/Resources/Level Blocks/" + block.name + ".prefab");

            // Saves the preview sprite of the object
            Texture2D previewTex = AssetPreview.GetAssetPreview(block);
            Sprite previewSpr = Sprite.Create(previewTex, new Rect(0, 0, previewTex.width, previewTex.height), Vector2.zero);
            previewSpr.name = block.name + "_spr";
            string assetPath = "Assets/Resources/Level Block Previews/" + previewSpr.name + ".png";
            System.IO.File.WriteAllBytes(Application.dataPath + "/../" + assetPath, previewSpr.texture.EncodeToPNG());
            AssetDatabase.Refresh();
            AssetDatabase.AddObjectToAsset(previewSpr, assetPath);
            AssetDatabase.SaveAssets();
        }
    }
}
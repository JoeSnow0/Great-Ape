using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConfigUI : EditorWindow {
    Color newColor;
    Font newFont;
    Transform target;
    string searchName;

    [MenuItem("UI/Config UI")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ConfigUI window = (ConfigUI)EditorWindow.GetWindow(typeof(ConfigUI));
        window.Show();
    }

    private void OnGUI()
    {
        target = (Transform)EditorGUILayout.ObjectField(target, typeof(Transform));
        searchName = EditorGUILayout.TextField(searchName);

        GUILayout.Label("Color");
        newColor = EditorGUILayout.ColorField(newColor);
        GUILayout.Label("Font");
        newFont = (Font)EditorGUILayout.ObjectField(newFont, typeof(Font));


        if (GUILayout.Button("Update color"))
        {
            
        }

        if(GUILayout.Button("Update font"))
        {

        }

        if(GUILayout.Button("Update all"))
        {

        }

        if(GUILayout.Button("Select search"))
        {
            GameObject[] selectedObjects = GameObject.FindGameObjectsWithTag(searchName);
            Selection.objects = selectedObjects;
        }

    }

    private void GetObjects()
    {

    }
}

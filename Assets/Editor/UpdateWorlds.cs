using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UpdateWorlds : EditorWindow {
    WorldsInfo worlds;

    [MenuItem("Window/Update Worlds")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        UpdateWorlds window = (UpdateWorlds)EditorWindow.GetWindow(typeof(UpdateWorlds));
        window.Show();
    }

    private void OnGUI()
    {
        worlds = (WorldsInfo)EditorGUILayout.ObjectField(worlds, typeof(WorldsInfo), false);
        if(GUILayout.Button("Update worlds"))
        {
            string[] levelFolders = Directory.GetDirectories("Assets/Resources/Levels");
            List<string> worldNames = new List<string>();
            for (int i = 0; i < levelFolders.Length; i++)
            {
                var split = levelFolders[i].Split(new char[] { '\\' });
                string folderName = split[split.Length - 1];
                string folderPath = split[0];
                worldNames.Add(folderName);
                LevelInfo[] l = Resources.LoadAll<LevelInfo>("Levels/" + folderName);
                for (int li = 0; li < l.Length; li++)
                {
                    l[li].world = i;
                }
            }
            worlds.worldNames = worldNames;
            AssetDatabase.SaveAssets();
        }
    }
}

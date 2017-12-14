using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelSaveManager
{
    // Path to the level save file
    private static string levelSavePath;
    public static string levelSaveFullPath
    {
        get { return levelSavePath;  }
    }

    // Decides if we need to save before creating a new or loading a level
    static bool m_changed = true;
    public static bool changed
    {
        get { return m_changed; }
    }

    // If we have loaded the level from a path we already now where it will be saved
    static bool m_loadedFromPath = false;
    public static bool loadedFromPath
    {
        get { return m_loadedFromPath;  }
    }

    // Acknowledges that a new level has been created
    public static void RecordNewLevel()
    {
        m_changed = true;
        m_loadedFromPath = false;

        levelSavePath = "";
    }

    // Add a function call to this whenever you make a change to the level
    public static void RecordChange()
    {
        m_changed = true;
    }

    // Decides that the level is saved
    public static void RecordSave(string savePath)
    {
        m_changed = false;
        m_loadedFromPath = true;

        levelSavePath = savePath;
    }
}

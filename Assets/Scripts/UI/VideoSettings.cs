using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]public class Settings
{
    public string key;
    public int value;
}


public class VideoSettings : MonoBehaviour {
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] GameObject fullscreenCheck;
    [SerializeField] GameObject vsyncCheck;

    Resolution[] resolutions;
    Resolution currentResolution = new Resolution();

    bool vsync;

    string savePath;
    string fileName = "Settings.ini";

    [SerializeField] List<Settings> settings = new List<Settings>();
    Dictionary<string, int> fileText = new Dictionary<string, int>();

    void Start () {

        InitFile();

        resolutions = Screen.resolutions;
        currentResolution.height = Screen.height;
        currentResolution.width = Screen.width;

        if(fileText != null && fileText.ContainsKey("vsync"))
            QualitySettings.vSyncCount = fileText["vsync"];
        if (QualitySettings.vSyncCount == 1)// Sets bool accordingly
            vsync = true;
        else
            vsync = false;

        if(KeyValueChanged("quality", QualitySettings.GetQualityLevel()))
        {
            QualitySettings.SetQualityLevel(fileText["quality"]);
        }

        qualityDropdown.value = QualitySettings.GetQualityLevel();

        foreach (Resolution res in resolutions)// Add all resolution options to dropdown menu
        {
            resolutionDropdown.AddOptions(new List<string> { res.width + "x" + res.height + " " + res.refreshRate + "hz"});
        }

        if (!Screen.fullScreen) {// Get current resolution
            for (int i = 0; i < resolutions.Length; i++)
            {
                if(currentResolution.height == resolutions[i].height && currentResolution.width == resolutions[i].width)
                {
                    resolutionDropdown.value = i;
                    break;
                }
                else
                {
                    resolutionDropdown.captionText.text = Screen.width + "x" + Screen.height + " " + resolutions[i].refreshRate + "hz";
                }
            }
        }
    }
	
	void Update()
    {
        vsyncCheck.SetActive(vsync);
        fullscreenCheck.SetActive(Screen.fullScreen);// Temp
    }

    void OnApplicationQuit()// Save the new settings to file when game is closed
    {
        if (fileText != null)
        {
            if (fileText.ContainsKey("fullscreen"))
                fileText["fullscreen"] = Convert.ToInt32(Screen.fullScreen);

            if (fileText.ContainsKey("vsync"))
                fileText["vsync"] = QualitySettings.vSyncCount;

            if (fileText.ContainsKey("quality"))
                fileText["quality"] = QualitySettings.GetQualityLevel();

            if (fileText.ContainsKey("width") && fileText.ContainsKey("height"))
            {
                fileText["width"] = currentResolution.width;
                fileText["height"] = currentResolution.height;
            }
        }

        StreamWriter writer = new StreamWriter(savePath, false);
        foreach (var entry in fileText)
        {
            writer.WriteLine(entry.Key + " " + entry.Value);
        }
        writer.Close();
    }

    public void UpdateResolution()// Sets resolution
    {
        currentResolution.width = resolutions[resolutionDropdown.value].width;
        currentResolution.height = resolutions[resolutionDropdown.value].height;
        currentResolution.refreshRate = resolutions[resolutionDropdown.value].refreshRate;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen, currentResolution.refreshRate);
    }

    public void UpdateFullscreen()// Toggles fullscreen/windowed
    {
        Screen.fullScreen = !Screen.fullScreen;

    }

    public void UpdateVsync()// Toggles vsync
    {
        vsync = !vsync;
        QualitySettings.vSyncCount = vsync ? 1 : 0;
    }

    public void UpdateQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    private bool KeyValueChanged(string key, int value)
    {
        if(fileText.ContainsKey(key) && fileText[key] != value)
        {
            return true;
        }
        return false;
    }

    private void InitFile()
    {
        savePath = Application.dataPath + "/" + fileName;

        if (!File.Exists(savePath))// Creates a new file if it doesn't exist
        {
            StreamWriter writer = new StreamWriter(savePath, false);
            foreach (Settings setting in settings)
            {
                writer.WriteLine(setting.key + " " + setting.value);
            }
            writer.Close();
        }

        bool match = true;
        StreamReader reader = new StreamReader(savePath);// Reads the file and checks if everything is there
        if (File.Exists(savePath))
        {
            string allText = reader.ReadToEnd();
            for (int i = 0; i < settings.Count; i++)
            {
                if (!allText.Contains(settings[i].key))
                {
                    match = false;
                    break;
                }
            }
            reader.Close();
        }

        if (!match)// Recreates the text content if something is missing
        {
            StreamWriter writer = new StreamWriter(savePath, false);
            foreach (Settings setting in settings)
            {
                writer.WriteLine(setting.key + " " + setting.value);
            }
            writer.Close();
        }


        reader = new StreamReader(savePath);
        string line = null;
        

        while((line = reader.ReadLine()) != null)// Adds the text content to a dictionary
        {
            string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                fileText.Add(parts[0], int.Parse(parts[parts.Length - 1]));
            }
        }
        reader.Close();

        /*for (int i = 0; i < settings.Count; i++)// Check if the keys match and sets the value from the text file to the settings class
        {
            int value = (fileText.ContainsKey(settings[i].key)) ? fileText[settings[i].key] : settings[i].value;
            settings[i].value = value;
        }*/
    }
}

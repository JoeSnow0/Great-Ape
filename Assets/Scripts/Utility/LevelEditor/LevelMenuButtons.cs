using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Events;

// Used for importing .dll for Windows Forms
//using System.Runtime.InteropServices;

public class LevelMenuButtons : MonoBehaviour
{
    [SerializeField]
    LevelEditorLoadSaveUtility loadSaveUtility;

    // When you press the "New Level..." button
    public void OnNewLevelButtonPressed()
    {
        // Checks if we want to save first
        UnityAction yesAction = () => OnSaveLevelAsButtonPressed();
        UnityAction noAction = () => LevelEditor.current.CreateNewLevel();
        if (CheckForUnsavedChanges("You have unsaved changes.\n Do you want to save before you create a new level?", yesAction, noAction))
            return;

        // If we didn't have any unsaved changes we create a new level
        LevelEditor.current.CreateNewLevel();
    }

    // When you press the "Load Level..." button
    public void OnLoadLevelButtonPressed()
    {
        // Checks if we want to save first
        UnityAction yesAction = (LevelSaveManager.loadedFromPath) ? new UnityAction(() => OnSaveLevelButtonPressed()) : new UnityAction(() => OnSaveLevelAsButtonPressed());
        UnityAction noAction = () => LoadLevel();
        if (CheckForUnsavedChanges("You have unsaved changes.\n Do you want to save before you load a level?", yesAction, noAction))
            return;
    }

    private void LoadLevel()
    { 
        // Windows forms OpenFileDialog
        OpenFileDialog dialog = new OpenFileDialog();

        // Asks for .sav files only
        dialog.Filter = "Level Save Files (*.sav)|*.sav";
        dialog.RestoreDirectory = true;
        dialog.InitialDirectory = UnityEngine.Application.dataPath + "/Saved Levels";

        // Gets path from dialog
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            string path = dialog.FileName;
            loadSaveUtility.LoadGame(path);

            YesNoDialog.current.CloseDialog();
        }
    }

    // When you press the "Save Level..." button
    public void OnSaveLevelButtonPressed()
    {
        // Checks if the level has a path already or if we need to set a location and name first
        if (!LevelSaveManager.loadedFromPath)
            OnSaveLevelAsButtonPressed();

        //loadSaveUtility.SaveGame
    }

    // Imports System.Windows.Forms.dll
    //[DllImport("user32.dll")]
    //private static extern void SaveFileDialog();

    // When you press the "Save Level as..." button
    public void OnSaveLevelAsButtonPressed()
    {
        SaveFileDialog dialog = new SaveFileDialog();

        // Asks for .sav files only
        dialog.Filter = "Level Save Files (*.sav)|*.sav";
        dialog.RestoreDirectory = true;
    }

    // When you press the "Exit to main menu" button
    public void OnExitToMainMenuButtonPressed()
    {
        //Debug.Log("Do you want to save your changes?");
        YesNoDialog.current.NewYesNoDialog("Do you really want to end the blog post?",
            () => Debug.Log("asdasd"), () => LevelEditor.current.CreateNewLevel());
    }

    // Checks if we have unsaved changes
    private bool CheckForUnsavedChanges(string dialogText, UnityAction yesAction, UnityAction noAction)
    {
        if (LevelSaveManager.changed)
        {
            // Asks if the user wants to save their changes first
            YesNoDialog.current.NewYesNoDialog("You have unsaved changes.\n Do you want to save before you create a new level?", yesAction, noAction);
        }

        return LevelSaveManager.changed;
    }
}

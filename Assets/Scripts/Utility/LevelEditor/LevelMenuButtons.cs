/////////////////////
///
/// Authored by: Oskar Svensson (Dec 15, 2017)
/// 
/// oskar0svensson@gmail.com
/// 
////////////////////

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Events;

// Used for importing .dll for Windows Forms
using System.Runtime.InteropServices;

public class LevelMenuButtons : MonoBehaviour
{
    [SerializeField]
    LevelEditorLoadSaveUtility loadSaveUtility;

    // When you press the "New Level..." button
    public void OnNewLevelButtonPressed()
    {
        // Checks if we want to save first
        UnityAction yesAction = () => OnSaveLevelButtonPressed();
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
        UnityAction yesAction = () => OnSaveLevelButtonPressed();
        UnityAction noAction = () => LoadLevel();
        if (CheckForUnsavedChanges("You have unsaved changes.\n Do you want to save before you load a level?", yesAction, noAction))
            return;

        LoadLevel();
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
            LevelObjectEditor.current.DeselectObject();

            string path = dialog.FileName;
            loadSaveUtility.LoadGame(path);

            LevelSaveManager.RecordSave(path);

            YesNoDialog.current.CancelDialog();
        }
        else
        {
            YesNoDialog.current.CloseDialog();
        }
    }

    // When you press the "Save Level..." button
    public void OnSaveLevelButtonPressed()
    {
        // Checks if the level has a path already or if we need to set a location and name first
        if (!LevelSaveManager.loadedFromPath)
        {
            OnSaveLevelAsButtonPressed();
            return;
        }
        LevelObjectEditor.current.DeselectObject();

        string fileName = Path.GetFileNameWithoutExtension(LevelSaveManager.levelSaveFullPath);
        //Debug.Log("Full Path: " + LevelSaveManager.levelSaveFullPath + ", Filename only: " + fileName);
        loadSaveUtility.SaveGame(LevelSaveManager.levelSaveFullPath, fileName);
        LevelSaveManager.RecordSave(LevelSaveManager.levelSaveFullPath);

        YesNoDialog.current.CancelDialog();
    }

    // Imports System.Windows.Forms.dll
    [DllImport("user32.dll")]
    private static extern void SaveFileDialog();
    [DllImport("user32.dll")]
    private static extern void OpenFileDialog();

    // When you press the "Save Level as..." button
    public void OnSaveLevelAsButtonPressed()
    {
        // Initializes SaveFileDialog with some values 
        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filter = "Level Save Files (*.sav)|*.sav";
        dialog.FileName = "newLevel.sav";
        dialog.RestoreDirectory = true;
        dialog.InitialDirectory = UnityEngine.Application.dataPath + "/Saved Levels";

        DialogResult result = dialog.ShowDialog();
        switch(result)
        { 
            case DialogResult.OK:
                LevelObjectEditor.current.DeselectObject();

                loadSaveUtility.SaveGame(dialog.FileName, Path.GetFileNameWithoutExtension(dialog.FileName));
                //Debug.Log("Save filed as: " + dialog.FileName);
                LevelSaveManager.RecordSave(dialog.FileName);

                YesNoDialog.current.CancelDialog();
            break;

            case DialogResult.Cancel:
                YesNoDialog.current.CloseDialog();
            break;
        }
    }

    // When you press the "Exit to main menu" button
    public void OnExitToMainMenuButtonPressed()
    {
        UnityAction yesAction = () => OnSaveLevelButtonPressed();
        UnityAction noAction = () => ExitToMainMenu();
        if (CheckForUnsavedChanges("You have unsaved changes.\n Do you want to save before exiting?", yesAction, noAction))
            return;
    }

    void ExitToMainMenu()
    {
        // Loads the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Checks if we have unsaved changes
    private bool CheckForUnsavedChanges(string dialogText, UnityAction yesAction, UnityAction noAction)
    {
        if (LevelSaveManager.changed)
        {
            // Asks if the user wants to save their changes first
            YesNoDialog.current.NewYesNoDialog(dialogText, yesAction, noAction);
        }

        return LevelSaveManager.changed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenuButtons : MonoBehaviour
{
    // When you press the "New Level..." button
    public void OnNewLevelButtonPressed()
    {
        /* if(unsavedChanges)
           {
               Dialog dialog = new Dialog("You have unsaved changes./n Do you want to save first?", 
                                           "Yes", (() => OnSaveLevelButtonPressed()), 
                                           "No", (() => LevelEditor.current.CreateNewLevel()));
           } */
        YesNoDialog.current.NewYesNoDialog("You have unsaved changes.\n Do you want to save before you create a new level?",
            () => Debug.Log("Save Dialog"), () => LevelEditor.current.CreateNewLevel());
    }

    // When you press the "Load Level..." button
    public void OnLoadLevelButtonPressed()
    {

    }

    // When you press the "Save Level..." button
    public void OnSaveLevelButtonPressed()
    {

    }

    // When you press the "Save Level as..." button
    public void OnSaveLevelAsButtonPressed()
    {

    }

    // When you press the "Exit to main menu" button
    public void OnExitToMainMenuButtonPressed()
    {
        Debug.Log("Do you want to save your changes?");
    }
}

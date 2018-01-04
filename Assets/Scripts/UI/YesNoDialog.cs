/////////////////////
///
/// Authored by: Oskar Svensson (Dec 15, 2017)
/// 
/// oskar0svensson@gmail.com
/// 
////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
public class YesNoDialog : MonoBehaviour
{
    private static YesNoDialog m_currentDialog;
    public static YesNoDialog current
    {
        get { return m_currentDialog; }
    }

    private GameObject m_mainCanvas;
    public GameObject mainCanvas
    {
        get { return m_mainCanvas; }
        set { m_mainCanvas = value; }
    }

    Canvas dialogCanvas;

    [SerializeField]
    Image screenshotImage;

    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Button yesButton;
    [SerializeField]
    Button noButton;
    [SerializeField]
    Button cancelButton;

    UnityAction noAction;

    bool wasYesPressed = false;

    private void Awake()
    {
        if (m_currentDialog == null)
            m_currentDialog = this;

        // Sets the canvas and deactivates it
        dialogCanvas = GetComponent<Canvas>();
        dialogCanvas.enabled = false;

        // Adds action to the cancel button
        cancelButton.onClick.AddListener(() => OnCancelButtonPress());
    }

    // Creates a new dialog based on the parameters sen in
    public void NewYesNoDialog(string description, UnityAction yesButtonCall, UnityAction noButtonCall)
    {
        // If there is already a dialog being shown, we ignore the new one
        if (dialogCanvas.isActiveAndEnabled == true)
            return;

        // TODO: Fix screenshot in background
        //screenshotImage = 
        // Deactivates main canvas
        m_mainCanvas.SetActive(false);

        // Changes description text
        descriptionText.text = description;

        // Removes the old listener call
        yesButton.onClick.RemoveAllListeners();
        // Adds the specified call to the button
        yesButton.onClick.AddListener(() => SetWasYesPressed(true));
        yesButton.onClick.AddListener(yesButtonCall);

        // The same as for the yes button
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => SetWasYesPressed(false));
        noButton.onClick.AddListener(noButtonCall);

        // Saves the action for the no button
        noAction = noButtonCall;

        dialogCanvas.enabled = true;
    }

    // Completely cancels the dialog's actions
    public void CancelDialog()
    {
        if (!dialogCanvas.enabled)
            return;

        OnCancelButtonPress();

        // If yes was pressed (to save the current level probably), 
        // we still want to do the other thing (like creating or loading a level)
        if (wasYesPressed)
            noAction();
    }

    // Cancel Button
    public void OnCancelButtonPress()
    {
        dialogCanvas.enabled = false;
        m_mainCanvas.SetActive(true);
    }

    public void CloseDialog()
    {
        OnCancelButtonPress();
    }

    // Is the dialog active or not
    public bool DialogActive()
    {
        return dialogCanvas.enabled;
    }

    private void SetWasYesPressed(bool yes)
    {
        wasYesPressed = yes;
    }
}

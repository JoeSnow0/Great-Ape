using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
public class YesNoDialog : MonoBehaviour
{
    private static YesNoDialog currentDialog;
    public static YesNoDialog current
    {
        get { return currentDialog; }
    }

    private Canvas m_mainCanvas;
    public Canvas mainCanvas
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

    private void Awake()
    {
        if (currentDialog == null)
            currentDialog = this;

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
        m_mainCanvas.gameObject.SetActive(false);

        // Removes the old listener call
        yesButton.onClick.RemoveAllListeners();
        // Adds the specified call to the button
        yesButton.onClick.AddListener(yesButtonCall);

        // The same as for the yes button
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(noButtonCall);

        dialogCanvas.enabled = true;
    }

    // Cancel Button
    void OnCancelButtonPress()
    {
        dialogCanvas.enabled = false;
        m_mainCanvas.gameObject.SetActive(true);
    }
}

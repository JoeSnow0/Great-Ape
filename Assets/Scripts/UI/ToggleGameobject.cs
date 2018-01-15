using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class ToggleGameobject : MonoBehaviour {
    [SerializeField] Keybindings keybindings;
    [SerializeField] Text levelName;
    private KeyCode escapeKey;
    GameObject childObject;
    MenuButtons[] menuButtons;

    PostProcessingBehaviour postProcess;

    private void Start()
    {
        levelName.text = SceneManager.GetActiveScene().name;
        
        childObject = transform.GetChild(0).gameObject;
        menuButtons = GetComponentsInChildren<MenuButtons>();
        if (Camera.main.GetComponent<PostProcessingBehaviour>() != null)
        {
            postProcess = Camera.main.GetComponent<PostProcessingBehaviour>();
            postProcess.profile.depthOfField.enabled = childObject.activeSelf;
        }
    }
    void Update () {
        escapeKey = keybindings.keybinding[keybindings.keybinding.Count - 1].keyValue;
        if (Input.GetKeyDown(escapeKey))
        {
            Toggle();

            
        }
    }

    public void Toggle()
    {
        foreach (MenuButtons button in menuButtons)
        {
            button.DisableWindows();
        }
        
        childObject.SetActive(!childObject.activeSelf);
        if (postProcess != null)
        {
            postProcess.profile.depthOfField.enabled = childObject.activeSelf;
        }
    }
}
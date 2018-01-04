using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindingsMenu : MonoBehaviour {
    List<Text> keybindingTexts = new List<Text>();
    [SerializeField] Keybindings[] profiles;

    public GameObject keybindingText;
    public Button button;
    bool binding = false;
    int currentKey;

    private KeyCode[] values;
    private bool[] keys;

    void Start () {

        values = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        keys = new bool[values.Length];

        UpdateKeyValues(profiles[0]);
	}
    
    void Update () {
        
        if (binding)// Checks if a button has been pressed
        {
            for (int i = 0; i < values.Length; i++)
            {
                keys[i] = Input.GetKey(values[i]);
                if (keys[i])
                {
                    keybindingTexts[currentKey].text = values[i].ToString();
                    profiles[0].keybinding[currentKey].keyValue = values[i];
                    binding = false;
                }
            }
        }
	}

    public void PressButton(GameObject buttonPressed)
    {
        for (int i = 0; i < buttonPressed.transform.parent.childCount; i++)
        {
            if (buttonPressed.transform.parent.GetChild(i) == buttonPressed.transform)
            {
                binding = true;
                keybindingTexts[i].text = "PRESS ANY KEY";
                currentKey = i;
                break;
            }
        }
    }

    public void ResetKeyValues()
    {
        if(keybindingTexts.Count > 0)// Checks if there are any buttons
        {
            for (int i = 0; i < profiles[0].keybinding.Count; i++)
            {
                profiles[0].keybinding[i].keyValue = profiles[1].keybinding[i].keyValue;
            }
        }
        UpdateKeyValues(profiles[0]);
    }

    private void UpdateKeyValues(Keybindings profile)
    {
        if (keybindingTexts.Count <= 0)
        {
            for (int i = 0; i < profile.keybinding.Count; i++)// Create all buttons and text fields
            {
                GameObject newButton = Instantiate(button, transform.parent.GetChild(transform.parent.childCount - 1)).gameObject;
                Text buttonText = newButton.GetComponentInChildren<Text>();
                buttonText.text = profile.keybinding[i].keyValue.ToString();
                newButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressButton(newButton.gameObject); });
                keybindingTexts.Add(buttonText);
                Instantiate(keybindingText, transform).GetComponentInChildren<Text>().text = profile.keybinding[i].actionName.ToString();
            }
        }
        else
        {
            for (int i = 0; i < keybindingTexts.Count; i++)// Updates all the texts
            {
                keybindingTexts[i].text = profile.keybinding[i].keyValue.ToString();
            }
        }
    }
}
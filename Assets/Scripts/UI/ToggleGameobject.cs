using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameobject : MonoBehaviour {
    [SerializeField] Keybindings keybindings;
    private KeyCode escapeKey;
    GameObject childObject;

    private void Start()
    {
        escapeKey = keybindings.keybinding[keybindings.keybinding.Count - 1].keyValue;
        childObject = transform.GetChild(0).gameObject;
    }
    void Update () {
        if (Input.GetKeyDown(escapeKey))
        {
            childObject.SetActive(!childObject.activeSelf);
        }
	}
}
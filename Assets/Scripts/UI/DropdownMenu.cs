using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropdownMenu : MonoBehaviour
{
    private bool showDropdown = false;

    [SerializeField]
    private GameObject content;
    private RectTransform content_RT;

    private Camera canvasCamera;

    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = EventSystem.current;

        content_RT = content.GetComponent<RectTransform>();
        canvasCamera = transform.root.GetComponent<Canvas>().worldCamera;

        // Adds it so when you click any of the buttons, the dropdown will be deactivated
        foreach(Button contentButton in content.GetComponentsInChildren<Button>())
        {
            contentButton.onClick.AddListener(() => ContentButtonClick(contentButton));
        }

        content.SetActive(showDropdown);
    }

    private void Update()
    {
        // If the dropdown is active and the mouse was clicked outside of the dropdown, we close it down
        if (showDropdown)
        {
            if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(content_RT, Input.mousePosition, canvasCamera))
                Dropdown(false);
        }
    }

    public void OnButtonClick()
    {
        if(!showDropdown)
            Dropdown(true);
    }

    // Sets the dropdown menu to be active or not
    private void Dropdown(bool active)
    {
        showDropdown = active;

        // Removes focus from the level menu button if it was pressed
        if (gameObject == eventSystem.currentSelectedGameObject)
            eventSystem.SetSelectedGameObject(null);

        // Sets the content to be shown or hidden
        content.SetActive(showDropdown);
    }

    // When we press a button in the content list, we deactivate the dropdown menu
    private void ContentButtonClick(Button contentButton)
    {
        // TODO: Fix so the buttons will become normal when you press away the dropdown menu, they are currently going to stay permanently pressed right now
        Dropdown(false);
        eventSystem.SetSelectedGameObject(null);
    }
}

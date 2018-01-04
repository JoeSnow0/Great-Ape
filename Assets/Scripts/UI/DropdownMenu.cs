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
using UnityEngine.EventSystems;

public class DropdownMenu : MonoBehaviour
{
    private bool m_showDropdown = false;
    public bool isDropdownActive
    {
        get { return m_showDropdown; }
    }

    [SerializeField]
    private GameObject content;
    private RectTransform m_content_RT;

    private Camera m_canvasCamera;

    private EventSystem m_eventSystem;

    private void Awake()
    {
        m_eventSystem = EventSystem.current;

        m_content_RT = content.GetComponent<RectTransform>();
        m_canvasCamera = transform.root.GetComponent<Canvas>().worldCamera;

        // Adds it so when you click any of the buttons, the dropdown will be deactivated
        foreach(Button contentButton in content.GetComponentsInChildren<Button>())
        {
            contentButton.onClick.AddListener(() => ContentButtonClick(contentButton));
        }

        content.SetActive(m_showDropdown);
    }

    private void Update()
    {
        // If the dropdown is active and the mouse was clicked outside of the dropdown, we close it down
        if (m_showDropdown)
        {
            if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(m_content_RT, Input.mousePosition, m_canvasCamera))
                Dropdown(false);
        }
    }

    public void OnButtonClick()
    {
        LevelObjectEditor.current.DeselectObject();
        if(!m_showDropdown)
            Dropdown(true);
    }

    // Sets the dropdown menu to be active or not
    private void Dropdown(bool active)
    {
        m_showDropdown = active;

        // Removes focus from the level menu button if it was pressed
        if (gameObject == m_eventSystem.currentSelectedGameObject)
            m_eventSystem.SetSelectedGameObject(null);

        // Sets the content to be shown or hidden
        content.SetActive(m_showDropdown);
    }

    // When we press a button in the content list, we deactivate the dropdown menu
    private void ContentButtonClick(Button contentButton)
    {
        // TODO: Fix so the buttons will become normal when you press away the dropdown menu, they are currently going to stay permanently pressed right now
        Dropdown(false);
        m_eventSystem.SetSelectedGameObject(null);
    }
}

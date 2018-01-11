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

public class LevelObjectEditor : MonoBehaviour
{
    private static LevelObjectEditor m_currentEditor;
    public static LevelObjectEditor current
    {
        get { return m_currentEditor; }
    }

    private GameObject currentObject;

    [SerializeField]
    LevelEditorTransformGizmo transformGizmo;

    private void Awake()
    {
        if(m_currentEditor == null)
            m_currentEditor = this;
    }


    private void Update()
    {
        if (currentObject == null)
            return;

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            GameObject c = currentObject;
            DeselectObject();
            DestroyObject(c);
            return;
        }

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
        {
            GameObject copiedObj = Instantiate(currentObject, currentObject.transform.position + Vector3.right, currentObject.transform.rotation);
            Destroy(copiedObj.GetComponent<cakeslice.Outline>());
            return;
        }
    }


    // Changes the selected object in the level
    public void ChangeSelection(GameObject newSelection)
    {
        if (newSelection == null)
            return;

        if (currentObject != null)
            DeselectObject();

        currentObject = newSelection;
        
        // Activates the inspector
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

        // Highlights the object with the Outline class
        currentObject.AddComponent<cakeslice.Outline>();
        ComponentManager.current.GenerateComponentValueList(currentObject);
        ChangeName(newSelection.name);

        transformGizmo.SetTransform(currentObject.transform);
    }

    // Deselects the currently selected object in the level
    public void DeselectObject()
    {
        if (currentObject == null)
            return;

        // Deactivates the inspector
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        // Destroys the Outline component on the currently selected objects
        Destroy(currentObject.GetComponent<cakeslice.Outline>());

        ComponentManager.current.ClearComponentValueList();

        transformGizmo.SetTransform(null);
    }


    public void OnNameChanged(Text newName)
    {
        if (currentObject == null)
            return;

        currentObject.name = newName.text;
    }

    void ChangeName(string newName)
    {
        if (currentObject == null)
            return;

        currentObject.name = newName;
    }
}

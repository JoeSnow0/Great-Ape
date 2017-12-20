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
    private static LevelObjectEditor currentEditor;
    public static LevelObjectEditor current
    {
        get { return currentEditor; }
    }

    private GameObject currentObject;

    List<System.Type> currentComponents = new List<System.Type>();

    private void Awake()
    {
        if(currentEditor == null)
            currentEditor = this;
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

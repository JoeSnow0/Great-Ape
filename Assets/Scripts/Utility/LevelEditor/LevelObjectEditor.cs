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
    // If the currentObject didn't have a Renderer of any kind we will have to use some child object
    private cakeslice.Outline currentOutline;

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
            Destroy(copiedObj.GetComponentInChildren<cakeslice.Outline>());
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
        currentOutline = HighlightObject(currentObject);

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
        //foreach(Transform child in gameObject.transform.GetChild(0))
        //{
        //    child.gameObject.SetActive(false);
        //}

        // Destroys the Outline component on the currently selected objects
        Destroy(currentOutline);

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

    private cakeslice.Outline HighlightObject(GameObject obj)
    {
        // Checks if the selected object has a renderer
        Renderer rend = obj.GetComponent<Renderer>();

        // If not...
        if(rend == null)
        {
            // Sets the first child with a renderer to have the outline
            rend = obj.GetComponentsInChildren<Renderer>()[0];
        }
        // Adds the outline to the selected object
        return rend.gameObject.AddComponent<cakeslice.Outline>();
    }
}

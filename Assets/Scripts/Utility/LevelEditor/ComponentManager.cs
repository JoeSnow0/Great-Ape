using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComponentManager : MonoBehaviour
{
    // The currently used ComponentManager
    private static ComponentManager m_currentManager;
    public static ComponentManager current
    {
        get { return m_currentManager; }
    }

    // GameObject in the scene that holds the list of all the component values, i.e the custom built level editor Inspector
    [SerializeField]
    Transform componentValueListHolder;

    delegate void TypeBindDelegate();
    Dictionary<System.Type, TypeBindDelegate> typeBindFunctions = new Dictionary<System.Type, TypeBindDelegate>();

    // The object that's currently being edited
    GameObject m_currentObject;

    // The Transform input fields of the current object
    InputField[] m_positionFields;
    InputField m_rotationField;
    InputField[] m_scaleFields;


#region Inspector Prefabs
    [SerializeField]
    GameObject gameObjectEntryPrefab;

    [SerializeField]
    GameObject componentEntryPrefab;

    [SerializeField]
    GameObject singleLineInputPrefab;

    [SerializeField]
    GameObject vector2InputPrefab;

    [SerializeField]
    GameObject vector3InputPrefab;

    [SerializeField]
    GameObject boolInputPrefab;

    #endregion

    private void Awake()
    {
        if (m_currentManager == null)
            m_currentManager = this;

        /* Adds binding functions to a dictionary with the relevant Type as key-value */
        typeBindFunctions.Add(typeof(Transform), BindValuesTransform);
    }

    // Removes all of the components in the componentHolder UI list
    public void ClearComponentValueList()
    {
        foreach(Transform child in componentValueListHolder)
        {
            Destroy(child.gameObject);
        }
    }

    // Gets the components of the current GameObject and tries to create a list of most of them
    public void GenerateComponentValueList(GameObject obj)
    {
        m_currentObject = obj;

        // Starts off by binding GameObject values at the top of the custom Inspector
        BindValuesGameObject();

        // Loops through all the components of the current GameObject
        foreach(Component c in m_currentObject.GetComponents<Component>())
        {
            // Identifies the spcific type of the component
            System.Type t = c.GetType();

            // Checks if the Level Editor has support for that specific type
            if (!typeBindFunctions.ContainsKey(t))
                continue;

            // Calls the binding function for the current type
            typeBindFunctions[t]();
        }
    }

    // Methods for getting values and binding them to UI components so you can change them in the Level Editor
#region Component Bind Methods
    
    // Creates input fields for the basic GameObject values
    private void BindValuesGameObject()
    {
        GameObject objEntry = Instantiate(gameObjectEntryPrefab, componentValueListHolder);
        
        // Gets the input field for changing the GameObject's name
        InputField nameInput = objEntry.GetComponentInChildren<InputField>();
        nameInput.onEndEdit.AddListener((string s) => m_currentObject.name = s);
        // Sets the start value
        nameInput.text = m_currentObject.name;

        //TODO: Make it so you can deactivate object while still seeing it
        //GameObject field = CreateBoolInputField(currentObject.activeSelf, (bool b) => currentObject.SetActive(b));
        //SetupInputField(field, "Active", objEntry.transform);
    }

    // Creates input fields for the Transform class' position, rotation and scale values
    private void BindValuesTransform()
    {
        Transform trans = m_currentObject.transform;

        GameObject componentEntry = Instantiate(componentEntryPrefab, componentValueListHolder);
        componentEntry.GetComponentInChildren<Text>().text = "Transform";

        // Binds the position of the selected object's transform and adds the vector3 input field as a child to the componentEntry
        #region Position Methods
        GameObject field = CreateVector2Field(trans.position,
        (string s) =>
        {
            float x;
            if (!float.TryParse(s, out x)) return;
            trans.position = new Vector3(x, trans.position.y, trans.position.z);
        },
        (string s) =>
        {
            float y;
            if (!float.TryParse(s, out y)) return;
            trans.position = new Vector3(trans.position.x, y, trans.position.z);
        });
        SetupInputField(field, "Position", componentEntry.transform);
        m_positionFields = field.GetComponentsInChildren<InputField>();
        #endregion

        // Binds the rotation of the selected object's transform and adds the vector3 input field as a child to the componentEntry
        #region Rotation Methods
        field = CreateSingleLineInputField(trans.eulerAngles.z.ToString(),
        (string s) =>
        {
            float z;
            if (!float.TryParse(s, out z)) return;
            trans.rotation = Quaternion.Euler(trans.eulerAngles.x, trans.eulerAngles.y, z);
        });
        SetupInputField(field, "Rotation", componentEntry.transform);
        m_rotationField = field.GetComponentInChildren<InputField>();
        #endregion

        // Binds the scale of the selected object's transform and adds the vector3 input field as a child to the componentEntry
        #region Scale Methods
        field = CreateVector2Field(trans.localScale,
        (string s) =>
        {
            float x;
            if (!float.TryParse(s, out x)) return;
            trans.localScale = new Vector3(x, trans.localScale.y, trans.localScale.z);
        },
        (string s) =>
        {
            float y;
            if (!float.TryParse(s, out y)) return;
            trans.localScale = new Vector3(trans.localScale.x, y, trans.localScale.z);
        });
        SetupInputField(field, "Scale", componentEntry.transform);

        m_scaleFields = field.GetComponentsInChildren<InputField>();

        #endregion

        UpdateTransformText(trans);
    }

    #endregion

    // Methods for adding listeners to UI elements

    #region UI Listener Methods

    // Method for setting parent of input field, scaling correctly and adding a label
    private void SetupInputField(GameObject field, string label, Transform parent)
    {
        field.transform.SetParent(parent);
        field.GetComponentInChildren<Text>().text = label;
        field.transform.localScale = Vector3.one;
        field.transform.localPosition = Vector3.zero;
    }

    // Creates an instance of the Vector2 input field prefab
    private GameObject CreateSingleLineInputField(string value, UnityAction<string> action)
    {
        GameObject singleLineInput = Instantiate(singleLineInputPrefab);

        // Gets the input field component in the prefab
        InputField field = singleLineInput.GetComponentInChildren<InputField>();
        
        field.text = value.ToString();
        field.onEndEdit.AddListener(action);

        return singleLineInput;
    }

    // Creates an instance of the Vector2 input field prefab
    private GameObject CreateVector2Field(Vector3 values, UnityAction<string> xAction, UnityAction<string> yAction)
    {
        GameObject vector2Input = Instantiate(vector2InputPrefab);

        // Places all the components fields and actions in arrays so we can easily loop through them
        InputField[] fields = vector2Input.GetComponentsInChildren<InputField>();
        UnityAction<string>[] actions = new UnityAction<string>[2] { xAction, yAction};
        for (int i = 0; i < 2; i++)
        {
            fields[i].text = values[i].ToString();
            fields[i].onEndEdit.AddListener(actions[i]);
        }

        return vector2Input;
    }

    // Creates an instance of the Vector3 input field prefab
    private GameObject CreateVector3Field(Vector3 values, UnityAction<string> xAction, UnityAction<string> yAction, UnityAction<string> zAction)
    {
        GameObject vector3Input = Instantiate(vector3InputPrefab);

        // Places all the components fields and actions in arrays so we can easily loop through them
        InputField[] fields = vector3Input.GetComponentsInChildren<InputField>();
        UnityAction<string>[] actions = new UnityAction<string>[3] { xAction, yAction, zAction };
        for(int i = 0; i < 3; i++)
        {
            fields[i].text = values[i].ToString();
            fields[i].onEndEdit.AddListener(actions[i]);
        }

        return vector3Input;
    }

    // Creates an instance of the bool input field prefab
    private GameObject CreateBoolInputField(bool value, UnityAction<bool> action)
    {
        GameObject boolInput = Instantiate(boolInputPrefab);

        Toggle toggle = boolInput.GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener(action);

        return boolInput;
    }
    #endregion

    // Sets the transform text values
    public void UpdateTransformText(Transform t)
    {
        m_rotationField.text = System.Math.Round(t.eulerAngles.z, 2).ToString();
        for(int i = 0; i < m_positionFields.Length; i++)
        {
            m_positionFields[i].text = System.Math.Round(t.position[i], 2).ToString();

            m_scaleFields[i].text = System.Math.Round(t.localScale[i], 2).ToString();
        }
    }
}

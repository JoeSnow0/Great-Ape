using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComponentManager : MonoBehaviour
{
    // The currently used ComponentManager
    private static ComponentManager currentManager;
    public static ComponentManager current
    {
        get { return currentManager; }
    }

    // GameObject in the scene that holds the list of all the component values, i.e the custom built level editor Inspector
    [SerializeField]
    Transform componentValueListHolder;

#region Inspector Prefabs
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
        if (currentManager == null)
            currentManager = this;
    }

    // Removes all of the components in the componentHolder UI list
    public void ClearComponentValueList()
    {
        foreach(Transform child in componentValueListHolder)
        {
            Destroy(child.gameObject);
        }
    }

    public List<System.Type> GenerateComponentValueList(GameObject obj)
    {
        List<System.Type> types = new List<System.Type>();

        // Tries to get a load of different components that are supported by the level editor

        // All GameObjects have a Transform so we can assume it has one
        types.Add(typeof(Transform));
        BindValuesTransform(obj.transform);


        return types;
    }

    // Methods for getting values and binding them to UI components so you can change them in the Level Editor
#region Component Bind Methods

    private void BindValuesTransform(Transform trans)
    {
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
        field.transform.SetParent(componentEntry.transform);
        field.GetComponentInChildren<Text>().text = "Position";
        field.transform.localScale = Vector3.one;
        field.transform.localPosition = Vector3.zero;
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
        field.transform.SetParent(componentEntry.transform);
        field.GetComponentInChildren<Text>().text = "Rotation";
        field.transform.localScale = Vector3.one;
        field.transform.localPosition = Vector3.zero;
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
        field.transform.SetParent(componentEntry.transform);
        field.GetComponentInChildren<Text>().text = "Scale";
        field.transform.localScale = Vector3.one;
        field.transform.localPosition = Vector3.zero;
        #endregion
    }

    #endregion

    // Methods for adding listeners to UI elements

    #region UI Listener Methods

    // Creates an instance of the Vector2 input field prefab
    private GameObject CreateSingleLineInputField(string value, UnityAction<string> action)
    {
        GameObject singleLineInput = Instantiate(singleLineInputPrefab);

        // Gets the input field component in the prefab
        InputField field = singleLineInput.GetComponentInChildren<InputField>();
        
        field.text = value.ToString();
        field.onEndEdit.AddListener(action);

        return singleLineInputPrefab;
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

    #endregion
}

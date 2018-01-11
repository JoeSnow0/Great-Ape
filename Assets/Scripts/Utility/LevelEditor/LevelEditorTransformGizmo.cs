using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorTransformGizmo : MonoBehaviour
{
    private Transform m_currentTransform;

    [SerializeField]
    Text stateText;

    [SerializeField]
    KeyCode translateKey = KeyCode.W;
    [SerializeField]
    KeyCode rotationKey = KeyCode.E;
    [SerializeField]
    KeyCode scaleKey = KeyCode.R;

    public enum TransformState
    {
        translate = 1,
        rotation = 2,
        scale = 3
    };

    TransformState state = TransformState.translate;

    public delegate void EmptyMethod();
    Dictionary<TransformState, EmptyMethod> updateMethods = new Dictionary<TransformState, EmptyMethod>();

    void Awake()
    {
        // Binds the TransformStates to their correct methods
        ResetUpdateMethods();
    }

    void Update()
    {
        if (m_currentTransform == null)
            return;

        UpdateTransformState();

        // Calls the currently selected tool update method
        if (Input.GetMouseButton(0))
        {
            updateMethods[state]();
        }
    }

    // The translate tool
    void UpdateTranslateTool()
    {
        m_currentTransform.Translate(-new Vector3(MouseHelper.delta.x / 75, MouseHelper.delta.y / 75, 0));
    }

    // The rotation tool
    void UpdateRotationTool()
    {
        Vector3 delta = MouseHelper.delta;
        float rotAmount = delta.x - delta.y;
        m_currentTransform.Rotate(new Vector3(0, 0, -rotAmount));
    }

    // The scaling tool
    void UpdateScaleTool()
    {
        Vector3 newScale = m_currentTransform.localScale;

        // Scales with mouse movement
        newScale -= new Vector3(MouseHelper.delta.x / 30, MouseHelper.delta.y / 30, 0);
        // Clamps scale to not go under 1 in the scaleable axis'
        newScale = new Vector3(Mathf.Max(newScale.x, 1), Mathf.Max(newScale.y, 1), newScale.z);

        m_currentTransform.localScale = newScale;
    }

    // Checks what Transform tool the user wants
    void UpdateTransformState()
    {
        if (Input.GetKeyDown(translateKey))
        {
            state = TransformState.translate;
            stateText.text = "Position";
        }
        else if (Input.GetKeyDown(rotationKey))
        {
            state = TransformState.rotation;
            stateText.text = "Rotation";
        }
        else if (Input.GetKeyDown(scaleKey))
        {
            state = TransformState.scale;
            stateText.text = "Scale";
        }
    }
    

    public void SetTransform(Transform t)
    {
        m_currentTransform = t;

        stateText.gameObject.SetActive(t != null);
    }

    // Binds the TransformStates to their correct methods
    public void ResetUpdateMethods()
    {
        updateMethods.Add(TransformState.translate, UpdateTranslateTool);
        updateMethods.Add(TransformState.rotation, UpdateRotationTool);
        updateMethods.Add(TransformState.scale, UpdateScaleTool);
    }

    // Adds callback methods to the tool updates
    public void BindMethodToToolUpdate(TransformState s, EmptyMethod method)
    {
        updateMethods[s] += method;
    }
}

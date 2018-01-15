using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorTransformGizmo : MonoBehaviour
{
    private Transform m_currentTransform;

    [SerializeField]
    ComponentManager manager;

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

        // Checks which transform tool the player wants to use
        UpdateTransformState();

        // Writes the correctly chosen tool and shows the current value being transformed
        UpdateText();

        // Calls the currently selected tool update method
        if (Input.GetMouseButton(0) && MouseHelper.delta.magnitude != 0)
        {
            updateMethods[state]();
            manager.UpdateTransformText(m_currentTransform);
        }
    }

    // The translate tool
    void UpdateTranslateTool()
    {
        Vector3 posDelta = new Vector3(MouseHelper.worldDelta.x, MouseHelper.worldDelta.y, 0);
        m_currentTransform.Translate(-posDelta, Space.World);
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
        // Scales with mouse movement
        Vector3 scaleDelta = new Vector3(MouseHelper.worldDelta.x, MouseHelper.worldDelta.y, 0);
        m_currentTransform.localScale -= scaleDelta;
        // Clamps scale to not go under 1 in the scaleable axis'
        m_currentTransform.localScale = new Vector3(Mathf.Max(m_currentTransform.localScale.x, 1), Mathf.Max(m_currentTransform.localScale.y, 1), m_currentTransform.localScale.z);
    }

    // Checks what Transform tool the user wants
    void UpdateTransformState()
    {
        if (Input.GetKeyDown(translateKey))
        {
            state = TransformState.translate;
        }
        else if (Input.GetKeyDown(rotationKey))
        {
            state = TransformState.rotation;
        }
        else if (Input.GetKeyDown(scaleKey))
        {
            state = TransformState.scale;
        }
    }
    
    void UpdateText()
    {
        switch(state)
        {
            case TransformState.translate:
                stateText.text = "Position";
                break;
            case TransformState.rotation:
                stateText.text = "Rotation";
                break;
            case TransformState.scale:
                stateText.text = "Scale";
                break;
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

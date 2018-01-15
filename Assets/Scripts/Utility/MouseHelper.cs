using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHelper : MonoBehaviour
{
    static Camera mouseCamera;

    private static Vector2 m_mousePos = Vector2.zero;
    public static Vector2 pos
    {
        get { return m_mousePos; }
    }

    private static Vector2 m_worldPos = Vector2.zero;
    public static Vector2 worldPos
    {
        get { return m_worldPos; }
    }

    private static Vector2 m_mouseDelta = Vector2.zero;
    public static Vector2 delta
    {
        get { return m_mouseDelta; }
    }

    private static Vector2 m_worldDelta = Vector2.zero;
    public static Vector2 worldDelta
    {
        get { return m_worldDelta; }
    }


    private void Awake()
    {
        if (mouseCamera == null)
            mouseCamera = GetComponent<Camera>();
    }


    private void Update()
    {
        UpdateMouseDelta();  
        UpdateWorldDelta();
    }


    private static void UpdateMouseDelta()
    {
        Vector2 newMousePos = Input.mousePosition;
        m_mouseDelta = m_mousePos - newMousePos;
        m_mousePos = newMousePos;
    }


    private static void UpdateWorldDelta()
    {
        Vector2 newMousePos = mouseCamera.ScreenToWorldPoint(m_mousePos);
        m_worldDelta = m_worldPos - newMousePos;
        m_worldPos = newMousePos;
    }

    // Checks if the mouse is moving towards or from a position
    public static bool MovingTowardsObject(Vector3 position)
    {
        Vector3 mouseTowards = new Vector3(m_worldPos.x - m_worldDelta.x, m_worldPos.y - m_worldDelta.y, position.z);
        
        // Checks if distance from new mouse position is smaller than the current mouse position
        return (Vector3.Distance(mouseTowards, position) < Vector3.Distance(new Vector3(m_worldPos.x, m_worldPos.y, position.z), position));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHelper : MonoBehaviour
{
    private static Vector2 m_mousePos = Vector2.zero;
    public static Vector2 pos
    {
        get { return m_mousePos; }
    }


    private static Vector2 m_mouseDelta = Vector2.zero;
    public static Vector2 delta
    {
        get { return m_mouseDelta; }
    }


    private void Update()
    {
        UpdateMouseDelta();  
    }


    private static void UpdateMouseDelta()
    {
        Vector3 newMousePos = Input.mousePosition;
        m_mouseDelta = m_mousePos - new Vector2(newMousePos.x, newMousePos.y);
        m_mousePos = Input.mousePosition;
    }
}

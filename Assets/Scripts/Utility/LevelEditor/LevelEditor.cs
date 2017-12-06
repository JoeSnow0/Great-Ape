/////////////////////
///
/// Authored by: Oskar Svensson (Dec xx, 2017)
/// 
/// oskar0svensson@gmail.com
/// 
////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    private static LevelEditor currentEditor;
    public static LevelEditor current
    {
        get { return currentEditor; }
    }

    [System.NonSerialized]
    public string currentBlock;

    [SerializeField]
    private GameObject levelHolder;

    private Level m_currentLevel;

    [SerializeField]
    private Camera canvasCamera;
    [SerializeField]
    private Camera levelCamera;
    [SerializeField]
    private RenderTexture levelTexture;

    private RectTransform m_RT;
    private Rect m_rect;
    List<RectTransform> list = new List<RectTransform>();

    private Vector3 mouseDelta = Vector3.zero;
    private Vector3 mousePos;

    private void Awake()
    {
        if (currentEditor == null)
            currentEditor = this;

        m_RT = GetComponent<RectTransform>();
        m_rect = m_RT.rect;

        m_currentLevel = levelHolder.transform.GetChild(0).GetComponent<Level>();

        mousePos = Input.mousePosition;
    }

    private void Update()
    {
        
        UpdateMouseDelta();

        // If the mouse is hovering over the main editor view
        if (!RectTransformUtility.RectangleContainsScreenPoint(m_RT, mousePos, canvasCamera))
            return;

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightMouseClick();
        }
        else if (Input.GetMouseButton(2))
        {
            MiddleMouseDown();
        }

        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
            MouseScrolled(scrollDelta);
    }

    // Handles Left mouse clicking inside of the editor view
    private void LeftMouseClick()
    {
        // Get the currently selected block
        currentBlock = LevelBlockSelect.current.GetCurrentBlock();

        Vector2 mouseInRect;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RT, Input.mousePosition, canvasCamera, out mouseInRect);
        // Shoots a ray from the screen with the mouse position
        Ray mouseRay = levelCamera.ScreenPointToRay(mouseInRect);
        RaycastHit hit;
        // Checks if the ray hit an object
        if (Physics.Raycast(mouseRay, out hit, 1 << 10))
        {
            //ClickOnObject(hit);
        }
        // If we don't click on an object
        else if (currentBlock != null)
        {
            // Creates a plane which has a normal towards the camera and we place it on the level's position
            Plane rayPlane = new Plane(-levelCamera.transform.forward, m_currentLevel.transform.position);
            float enter;
            if (rayPlane.Raycast(mouseRay, out enter))
            {
                // Gets the position where the ray hit the point
                Vector3 spawnPos = mouseRay.GetPoint(enter);
                Debug.Log("Mouse Pos: " + mousePos + ", Spawn Pos: " + levelCamera.WorldToScreenPoint(spawnPos));
                Object blockPrefab = Resources.Load("Level Blocks/" + currentBlock, typeof(GameObject));
                GameObject newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, m_currentLevel.transform) as GameObject;
            }
        }


    }

    // Handles Right mouse clicking inside of the editor view
    private void RightMouseClick()
    {

    }

    // Handles Middle mouse clicking inside of the editor view
    private void MiddleMouseClick()
    {

    }

    // Handles when the Middle mouse button is down
    private void MiddleMouseDown()
    {
        Vector2 mDelta = (mouseDelta / 50);
        levelCamera.transform.position -= new Vector3(-mDelta.x, mDelta.y, 0);
    }

    // Handles when the mouse was scrolled
    private void MouseScrolled(float scrollDelta)
    {
        float orthoSize = Mathf.Clamp(levelCamera.orthographicSize - scrollDelta, 1, 100);
        levelCamera.orthographicSize = orthoSize;
    }

    private void UpdateMouseDelta()
    {
        Vector3 newMousePos = Input.mousePosition;
        mouseDelta = mousePos - newMousePos;
        mousePos = newMousePos;
    }
}

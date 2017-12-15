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
    public Level currentLevel
    {
        get { return m_currentLevel; }
    }

    private Camera canvasCamera;
    [SerializeField]
    private Camera levelCamera;
    [SerializeField]
    private RenderTexture levelTexture;

    private RectTransform m_RT;

    private Vector3 mouseDelta = Vector3.zero;
    private Vector3 mousePos;

    [SerializeField]
    DropdownMenu dropDown;

    private void Awake()
    {
        if (currentEditor == null)
            currentEditor = this;

        // Gets the canvas of the camera
        canvasCamera = transform.root.GetComponent<Canvas>().worldCamera;

        m_RT = GetComponent<RectTransform>();

        CreateNewLevel();

        mousePos = Input.mousePosition;
    }

    private void Start()
    {
        YesNoDialog.current.mainCanvas = transform.root.gameObject;
    }

    private void Update()
    {
        UpdateMouseDelta();

        // You should not be able to interact with the editor while the dropdown is open
        if (dropDown.isDropdownActive)
            return;

        // If the mouse is hovering over the main editor view
        if (!RectTransformUtility.RectangleContainsScreenPoint(m_RT, mousePos, canvasCamera))
            return;

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseDown();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightMouseDown();
        }
        else if (Input.GetMouseButton(2))
        {
            MiddleMouse();
        }

        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
            MouseScrolled(scrollDelta);
    }

    // Handles Left mouse clicking inside of the editor view
    private void LeftMouseDown()
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

                Object blockPrefab = Resources.Load("Level Blocks/" + currentBlock, typeof(GameObject));
                GameObject newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, m_currentLevel.transform) as GameObject;
                newBlock.layer = 10;

                // Records that their has been a change in the level
                LevelSaveManager.RecordChange();
            }
        }


    }

    // Handles Right mouse clicking inside of the editor view
    private void RightMouseDown()
    {
        
    }

    // Handles Middle mouse clicking inside of the editor view
    private void MiddleMouseDown()
    {

    }

    // Handles when the Middle mouse button is down
    private void MiddleMouse()
    {
        Vector2 mDelta = (mouseDelta / 50);
        levelCamera.transform.position -= new Vector3(mDelta.x, -mDelta.y, 0);
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

    // Creates a new level
    public void CreateNewLevel()
    {   
        //HACK: We should only set the currentlevel while debugging the level editor, remove this line when building
        m_currentLevel = levelHolder.transform.GetComponentInChildren<Level>();
        
        // Destroys the old level first
        // The old level should already be saved by this point
        if (m_currentLevel != null)
        {
            foreach(Transform levelBlock in m_currentLevel.transform)
            {
                Destroy(levelBlock.gameObject);
            }
            Destroy(m_currentLevel.gameObject);
        }

        // Creates new Level and sets a parent to it
        GameObject levelObj = Instantiate(Resources.Load("Level Prefabs/Empty Level", typeof(Object)) as GameObject);
        m_currentLevel = levelObj.AddComponent<Level>();
        levelObj.transform.SetParent(levelHolder.transform);
       
        // Resets the position of the level to be at 0,0,0 locally
        m_currentLevel.transform.localPosition = Vector3.zero;

        // Records that a new level has been created
        LevelSaveManager.RecordNewLevel();
    }

    // Switches the current level to the one loaded in
    public void LoadLevel(Level loadLevel)
    {
        // Destroys the old level
        foreach (Transform levelBlock in m_currentLevel.transform)
        {
            Destroy(levelBlock.gameObject);
        }
        Destroy(m_currentLevel.gameObject);

        // Sets the loaded level to be held by the levelholder at the local position (0,0,0)
        loadLevel.transform.SetParent(levelHolder.transform);
        loadLevel.transform.localPosition = Vector3.zero;

        m_currentLevel = loadLevel;
    }
}

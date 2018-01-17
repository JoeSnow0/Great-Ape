using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    // The currently used LevelEditor
    private static LevelEditor m_currentEditor;
    public static LevelEditor current
    {
        get { return m_currentEditor; }
    }

    // The currently selected block prefab
    [System.NonSerialized]
    public string currentBlock;

    // The object holding the Level object we are editing
    [SerializeField]
    private GameObject levelHolder;

    // The current Level that's being edited
    private Level m_currentLevel;
    public Level currentLevel
    {
        get { return m_currentLevel; }
    }

    // The camera displaying the canvas of the editor
    private Camera canvasCamera;

    // The camera displaying the actual level
    [SerializeField]
    private Camera levelCamera;
    private Vector3 m_orgCamerPos;
    private bool m_followingObject;

    [SerializeField]
    private RenderTexture levelTexture;

    private RectTransform m_RT;

    // The object that's currently being edited
    private GameObject currentObject;
    public GameObject currentSelection
    {
        get { return currentObject; }
    }

    [SerializeField]
    DropdownMenu dropDown;

    [SerializeField]
    [Range(0.0001f, 0.01f)]
    float cameraSpeed;

    const int MIN_ORTHO_SIZE = 10;
    const int MAX_ORTHO_SIZE = 300;

    private void Awake()
    {
        if (m_currentEditor == null)
            m_currentEditor = this;

        // Gets the canvas of the camera
        canvasCamera = transform.root.GetComponent<Canvas>().worldCamera;

        m_orgCamerPos = levelCamera.transform.position;

        m_RT = GetComponent<RectTransform>();

        CreateNewLevel();
    }

    private void Start()
    {
        YesNoDialog.current.mainCanvas = transform.root.gameObject;
    }
    private void Update()
    {
        // You should not be able to interact with the editor while the dropdown is open
        if (dropDown.isDropdownActive)
            return;

        // If the mouse is hovering over the main editor view
        if (!RectTransformUtility.RectangleContainsScreenPoint(m_RT, MouseHelper.pos, canvasCamera))
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

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
            levelCamera.transform.position = m_orgCamerPos;
        else if (Input.GetKeyDown(KeyCode.F) && !m_followingObject)
        {
            m_followingObject = true;
            StartCoroutine(MoveCameraToObject());
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
        Ray mouseRay = levelCamera.ScreenPointToRay(mouseInRect);
        // Checks if the ray hit an object
        RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        
        if(hit.collider != null)
        {
            currentObject = hit.collider.gameObject;
            LevelObjectEditor.current.ChangeSelection(currentObject);
        }
        // If we don't click on an object
        else
        {
            // Checks if we already have an object selected
            if(currentObject != null)
            {
                currentObject = null;
                m_followingObject = false;
                LevelObjectEditor.current.DeselectObject();
                return;
            }

            // Checks if we can place a block prefab
            if (currentBlock == null)
                return;

            // Creates a plane which has a normal towards the camera and we place it on the level's position
            Plane rayPlane = new Plane(-levelCamera.transform.forward, m_currentLevel.transform.position);
            float enter;
            if (rayPlane.Raycast(mouseRay, out enter))
            {
                // Gets the position where the ray hit the point
                Vector3 spawnPos = mouseRay.GetPoint(enter);

                Object blockPrefab = Resources.Load("Level Blocks/" + currentBlock, typeof(GameObject));
                GameObject newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, m_currentLevel.transform) as GameObject;

                // Selects the object in the custom objecteditor
                LevelObjectEditor.current.ChangeSelection(newBlock);
                currentObject = newBlock;

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
        Vector2 mDelta = (MouseHelper.delta * levelCamera.orthographicSize) * cameraSpeed;
        levelCamera.transform.position -= new Vector3(-mDelta.x, -mDelta.y, 0);
        m_followingObject = false;
    }

    // Handles when the mouse was scrolled
    private void MouseScrolled(float scrollDelta)
    {
        scrollDelta *= levelCamera.orthographicSize / MIN_ORTHO_SIZE;
        float orthoSize = Mathf.Clamp(levelCamera.orthographicSize - scrollDelta, MIN_ORTHO_SIZE, MAX_ORTHO_SIZE);
        levelCamera.orthographicSize = orthoSize;
    }

    // Moves the camera to the currently selected object
    private IEnumerator MoveCameraToObject()
    {
        if (currentObject == null)
            yield return null;

        Transform camTrans = levelCamera.transform;
        // The object we're following
        GameObject fObject = currentObject;
        Transform objTrans = currentObject.transform;

        // Checking if the following was not interrupted in any way such as deleting the object or switching object
        while(m_followingObject && currentObject != null || currentObject == fObject)
        {
            // Lerps the camera to the object's x and y positions
            levelCamera.transform.position = new Vector3(Mathf.Lerp(camTrans.position.x, objTrans.position.x, 0.25f),
                                                         Mathf.Lerp(camTrans.position.y, objTrans.position.y, 0.25f),
                                                         camTrans.position.z);

            // Checks if the camera is close enough
            if (Vector2.Distance(new Vector2(camTrans.position.x, camTrans.position.y), new Vector2(objTrans.position.x, objTrans.position.y)) < 0.1f)
                break;
        }
        m_followingObject = false;
        yield return null;
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
        GameObject levelObj = Instantiate(Resources.Load("Level Prefabs/EmptyLevelPrefab", typeof(Object)) as GameObject);
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

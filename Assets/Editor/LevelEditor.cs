/////////////////////
///
/// Authored by: Oskar Svensson (Dec xx, 2017)
/// 
/// oskar0svensson@gmail.com
/// 
////////////////////

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


public class LevelEditor : EditorWindow
{
    // The size of the Texture2D returned from the method AssetPreview.GetAssetPreview()
    const int PREVIEW_SIZE_X = 128;
    const int PREVIEW_SIZE_Y = 128;

    Dictionary<string, Level> m_levelDictionary = new Dictionary<string, Level>();
    
    // The current level being worked on
    Level level;
    // The original layer the level had
    int levelLayer;

    // The key is the path of the object the texture is representing
    Dictionary<string, Texture2D> m_previewTextures = new Dictionary<string, Texture2D>();

    // The string key of the block currently selected
    private string m_currentBlockKey = "";

    /* Values used in GUI elements */
    Vector2 m_scrollPosition = Vector2.zero;
    int m_selectIndex = 0;
    string m_levelBlockSearch = "";

    GameObject m_cameraObject;
    Vector3 m_orgPos;
    Camera m_cam;
    float orthoSize = 30f;

    bool m_createObject = false;

    [MenuItem("Window/Level Editor")]
    static void Init()
    {
        LevelEditor editor = (LevelEditor)EditorWindow.GetWindow<LevelEditor>();
        editor.titleContent = new GUIContent("Level Editor");

        // Gets all the Level objects in the scene
        Level[] levels = GameObject.FindObjectsOfType(typeof(Level)) as Level[];
        foreach (Level level in levels)
        {
            editor.m_levelDictionary.Add(level.gameObject.name, level);
        }
    }


    private void OnGUI()
    {
        // Draws elements that should always be on screen
        DrawEditorMenu();

        // Checks if the user has selected a level now
        if (level != null)
        {
            // Allows the user to move the camera around
            ControlCamera();

            // Draws the camera in this Editor Window
            DrawCamera();

            // Draws a preview of the selected object in the corner
            DrawSelectedPreview();
        }

        // Draws a dropdown menu for selecting GameObjects with a level component on them in the scene
        DrawLevelSelect();
    }

    // Takes care of creating everything that needs to be in the window
    private void DrawEditorMenu()
    {
        // Gets all the level blocks in the project folder
        if (m_previewTextures.Count <= 0)
        {
            UpdateLevelBlockPreviews();
        }
      
        // Draws the level blocks in a scroll view and gets rects based on all the preview images
        DrawLevelBlockList();
    }

    // Draws a camera view in the editor window
    private void DrawCamera()
    {
        // Manually Renders the camera and draws the RenderTexture in the camera
        m_cam.Render();
        GUI.DrawTexture(new Rect(0, 0, position.width - PREVIEW_SIZE_X - 15, position.height), m_cam.targetTexture);
    }

    // Controls the camera in the editor window
    private void ControlCamera()
    {
        Event current = Event.current;

        switch(current.type)
        {
            // Moves the camera when we drag the mouse
            case EventType.MouseDrag:
                Vector2 mouseDelta = current.delta;
                m_cameraObject.transform.position -= new Vector3(mouseDelta.x, -mouseDelta.y);
                m_createObject = false;
                break;

            // Sets so when the user releases the button they will create an object
            case EventType.MouseDown:
                if (current.button == 0 && m_currentBlockKey.Length > 0)
                {
                    m_createObject = true;
                }
                break;

            // Creates the selected object
            case EventType.MouseUp:
                if (m_createObject)
                {
                    // Shoots a ray from the screen with the mouse position
                    Ray mouseRay = m_cam.ScreenPointToRay(new Vector2(current.mousePosition.x, position.height - current.mousePosition.y));
                    // Creates a plane which has a normal towards the camera and we place it on the level's position
                    Plane rayPlane = new Plane(-m_cam.transform.forward, level.transform.position);
                    float enter;
                    if (rayPlane.Raycast(mouseRay, out enter))
                    {
                        // Gets the position where the ray hit the point
                        Vector3 spawnPos = mouseRay.GetPoint(enter);
                        // Gets the orefab with the path in currentBlockKey
                        Object prefab = Resources.Load(m_currentBlockKey, typeof(GameObject));
                        // Instantiates the prefab and sets the level as it's parent
                        GameObject block = Instantiate(prefab, spawnPos, Quaternion.identity) as GameObject;
                        block.layer = 8;
                        block.transform.SetParent(level.transform);
                    }
                }

                break;

            case EventType.ScrollWheel:
                m_cam.orthographicSize += current.delta.y;
                orthoSize = m_cam.orthographicSize;
                break;

            case EventType.KeyDown:
                if (current.keyCode == KeyCode.R && m_cameraObject.transform.position != m_orgPos)
                {
                    // Resets the camera to it's original position
                    m_cameraObject.transform.position = m_orgPos;
                }
                else
                    return;
                break;

            default:
                return;
        }

        Repaint();
    }

    // Displays a scroll view with all the available level blocks
    private void DrawLevelBlockList()
    {
        float originPosX = position.width - PREVIEW_SIZE_X - 15;
        Rect areaSize = new Rect(originPosX, 0, PREVIEW_SIZE_X + 15, position.height);

        GUILayout.BeginArea(new Rect(areaSize));

        // A search window for the level block select
        //TODO: Make it so you can focus and unfocus the textbox. IMPORTANT!!!
        Rect textFieldRect = new Rect(0, 0, areaSize.width, 20);
        m_levelBlockSearch = GUI.TextField(textFieldRect, m_levelBlockSearch);

        if(m_levelBlockSearch.Length <= 0)
        {
            Color orgColor = GUI.color;
            GUI.color = Color.grey;
            GUI.Label(textFieldRect, "Search...");
            GUI.color = orgColor;
        }

        // Rects for the scroll area with the level prefabs
        Rect viewport = new Rect(0, textFieldRect.height, PREVIEW_SIZE_X + 15, areaSize.height - textFieldRect.height);
        Rect contentSize = new Rect(0, 0, viewport.width - 15, PREVIEW_SIZE_Y * m_previewTextures.Count);

        // Starts a scroll list for all the level blocks
        m_scrollPosition = GUI.BeginScrollView(viewport, m_scrollPosition, contentSize);

        // Draws the preview texture of all the level blocks
        float yPos = 0;
        foreach (KeyValuePair<string, Texture2D> preview in m_previewTextures)
        {
            // If the search string doesn't match the object's name we skip it
            if (!preview.Key.ToLower().Contains(m_levelBlockSearch.ToLower()))
                continue;

            // Creates a button with the current preview image
            if(GUI.Button(new Rect(0, yPos, PREVIEW_SIZE_X, PREVIEW_SIZE_Y), preview.Value))
            {
                UpdateSelectedBlock(preview.Key);
            }
            yPos += preview.Value.height;
        }

        GUI.EndScrollView();
        GUILayout.EndArea();
    }

    // Updates the list of level blocks
    private void UpdateLevelBlockPreviews()
    {
        // Gets all the GameObjects in the folder at "Resources/Level Blocks"
        Object[] blockPrefabs = Resources.LoadAll<GameObject>("Level Blocks");

        foreach(Object block in blockPrefabs)
        {
            // We save the path for later when we want to instantiate that level block
            string path = "Level Blocks/" + block.name;
            // Gets a preview of the asset that showcases it like in the project view
            Texture2D preview = AssetPreview.GetAssetPreview(block);

            m_previewTextures.Add(path, preview);
        }
    }

    // Draws the preview selected in the button list
    private void DrawSelectedPreview()
    {
        if (m_currentBlockKey.Length <= 0)
            return;

        // Draws the selected levelblock preview in the upper left corner and also the name of the object
        Texture2D selectedPreview = m_previewTextures[m_currentBlockKey];
        Rect previewRect = new Rect(0, 20, selectedPreview.width, selectedPreview.height);

        GUIStyle style = GUIStyle.none;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, 0, selectedPreview.width, 20), Resources.Load(m_currentBlockKey).name, style);

        Color orgColor = GUI.color;
        GUI.color = new Color(1, 1, 1, 0.5f);
        GUI.DrawTexture(previewRect, selectedPreview);
        GUI.color = orgColor;
    }


    private void UpdateSelectedBlock(string selectedBlock)
    {
        m_currentBlockKey = selectedBlock;
    }


    private void DrawLevelSelect()
    {
        string[] popupOptions = m_levelDictionary.Keys.ToArray();

        GUILayout.BeginArea(new Rect(5, position.height - 60, 150, 60));
        
        EditorGUILayout.LabelField("Selected Level:");
        m_selectIndex = EditorGUI.Popup(new Rect(0, 15, 140, 50), m_selectIndex, popupOptions);

        GUILayout.EndArea();

        Level selectedLevel = null;
        if (m_selectIndex < popupOptions.Length)
             selectedLevel = m_levelDictionary[popupOptions[m_selectIndex]];


        if (selectedLevel != level && selectedLevel != null)
        {
            if(level != null)
            {
                level.gameObject.layer = levelLayer;
                for (int i = 0; i < level.transform.childCount; i++)
                {
                    level.transform.transform.GetChild(i).gameObject.layer = levelLayer;
                }
            }
            // Sets the new level
            level = selectedLevel;
            levelLayer = level.gameObject.layer;
            level.gameObject.layer = 8;
            for (int i = 0; i < level.transform.childCount; i++)
            {
                level.transform.transform.GetChild(i).gameObject.layer = level.gameObject.layer;
                Debug.Log(level.transform.GetChild(i).gameObject.layer);
            }

            // Destroys the old camera if there was one
            if (m_cameraObject != null)
            {
                DestroyImmediate(m_cameraObject);
            }
            // Creates a camera for the selected level
            CreateCamera();

            // MAYBE PUT AN UPDATE TO GET ALL THE LEVEL OBJECTS FROM THE SCENE AGAIN
        }
    }

    // Creates a camera object based on the position of the level selected
    private void CreateCamera()
    {
        m_cameraObject = new GameObject("Level Editor Camera");
        m_cameraObject.hideFlags = HideFlags.HideInHierarchy;
        m_cameraObject.transform.position = level.transform.position - Vector3.forward * 25;
        m_orgPos = m_cameraObject.transform.position;

        m_cam = m_cameraObject.AddComponent<Camera>();
        m_cam.clearFlags = CameraClearFlags.Color;
        m_cam.backgroundColor = Color.grey;
        m_cam.cullingMask = 1 << 8;
        m_cam.orthographic = true;
        m_cam.orthographicSize = orthoSize;

        // Creates a new RenderTexture and sets it as the camera's target
        m_cam.targetTexture = new RenderTexture((int)position.width - PREVIEW_SIZE_X, (int)position.height, 16, RenderTextureFormat.ARGB32);
    }


    // When the window is destroyed
    private void OnDestroy()
    {
        if(level != null)
        {
            level.gameObject.layer = levelLayer;
            for (int i = 0; i < level.transform.childCount; i++)
            {
                level.transform.GetChild(i).gameObject.layer = levelLayer;
            }
        }

        // Destroys and clears all relevant objects
        // TODO: Remove all the previewtextures
        m_previewTextures.Clear();
        m_levelDictionary.Clear();

        //DestroyImmediate(cameraTex);
        DestroyImmediate(m_cameraObject);
    }
}

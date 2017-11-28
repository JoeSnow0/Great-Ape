using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelEditor : EditorWindow
{
    // The size of the Texture2D returned from the method AssetPreview.GetAssetPreview()
    const int PREVIEW_SIZE_X = 128;
    const int PREVIEW_SIZE_Y = 128;

    // The current level being worked on
    Level level;

    // The key is the path of the object the texture is representing
    Dictionary<string, Texture2D> previewTextures = new Dictionary<string, Texture2D>();

    // The string key of the block currently selected
    private string currentBlockKey = "";

    [ReadOnly]
    Vector2 scrollPosition = Vector2.zero;

    Camera camera;


    [MenuItem("Window/Level Editor")]
    static void Init()
    {
        LevelEditor editor = (LevelEditor)EditorWindow.GetWindow<LevelEditor>();
        editor.titleContent = new GUIContent("Level Editor");
        editor.Show();
    }


    private void OnGUI()
    {
        // Creates all the important elements of the editor such as the grid and object picker
        InitializeEditor();

        // Gets the current level instance and checks if it is null
        if (Selection.activeGameObject != null)
        {
            level = Selection.activeGameObject.GetComponent<Level>();
            if (level == null)
                return;
        }

        // Draws a preview of the selected object in the corner
        DrawSelectedPreview();
    }

    // When the window is destroyed
    private void OnDestroy()
    {
        // Clears the preview textures and their respective rects
        previewTextures.Clear();
    }

    // Takes care of creating everything that needs to be in the window
    private void InitializeEditor()
    {
        // Gets all the level blocks in the project folder
        if (previewTextures.Count <= 0)
        {
            UpdateLevelBlockPreviews();
        }
      
        // Draws the level blocks in a scroll view and gets rects based on all the preview images
        DrawLevelBlockList();
    }

    // Displays a scroll view with all the available level blocks
    private void DrawLevelBlockList()
    {
        float originPosX = position.width - PREVIEW_SIZE_X;
        Rect viewport = new Rect(originPosX, 0, PREVIEW_SIZE_X, position.height);
        Rect contentSize = new Rect(0, 0, viewport.width, PREVIEW_SIZE_Y * previewTextures.Count);

        scrollPosition = GUI.BeginScrollView(viewport, scrollPosition, contentSize);
        // Draws the preview texture of all the level blocks
        float yPos = 0;
        foreach (KeyValuePair<string, Texture2D> preview in previewTextures)
        {
            // Creates a button with the current preview image
            if(GUI.Button(new Rect(0, yPos, PREVIEW_SIZE_X, PREVIEW_SIZE_Y), preview.Value))
            {
                UpdateSelectedBlock(preview.Key);
            }

            //GUI.DrawTexture(new Rect(0, yPos, preview.Value.width, preview.Value.height), preview.Value);
            yPos += preview.Value.height;
        }

        GUI.EndScrollView();
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

            previewTextures.Add(path, preview);
        }
    }

    // Draws the preview selected in the button list
    private void DrawSelectedPreview()
    {
        if (currentBlockKey.Length <= 0)
            return;

        Texture2D selectedPreview = previewTextures[currentBlockKey];
        Rect previewRect = new Rect(0, 20, selectedPreview.width, selectedPreview.height);

        GUIStyle style = GUIStyle.none;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, 0, selectedPreview.width, 20), Resources.Load(currentBlockKey).name, style);

        Color orgColor = GUI.color;
        GUI.color = new Color(1, 1, 1, 0.5f);
        GUI.DrawTexture(previewRect, selectedPreview);
        GUI.color = orgColor;

        Object obj = Resources.Load<GameObject>(currentBlockKey);
        GameObject temp = Instantiate(obj) as GameObject;

        camera = Camera.current;
        camera.targetTexture = new RenderTexture((int)position.width - PREVIEW_SIZE_X, (int)position.height, 24, RenderTextureFormat.ARGB32);
        camera.Render();

        DestroyImmediate(temp);
    }

    private void UpdateSelectedBlock(string selectedBlock)
    {
        currentBlockKey = selectedBlock;
    }
}

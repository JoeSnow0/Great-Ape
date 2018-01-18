using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    GameObject mainground;

    [SerializeField]
    GameObject background;

    [SerializeField]
    GameObject menuHolder;

    const int COIN_COUNT = 3;
    [SerializeField]
    GreatCoinScript[] coins = new GreatCoinScript[COIN_COUNT];

    [SerializeField]
    TextMesh startText;

    [SerializeField]
    TextMesh goalText;

    [SerializeField]
    ApeSelectionController apeScript;

    [SerializeField]
    Camera gameCamera;

    [SerializeField]
    GameObject apeArrow;

    bool m_testingLevel = false;

    void OnLoad()
    {
        LevelEditor.current.LoadLevel(this);
    }

    void Update()
    {
        if (!m_testingLevel)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTesting(true);
        }
    }

    public void ToggleTesting(bool stop)
    {
        m_testingLevel = !stop;

        if (stop)
        {
            apeScript.RemoveAllApes();
        }

        PrepareLevel(!stop);
    }

    // Prepares objects by de/activating them for the intended purpose
    public void PrepareLevel(bool forSaving)
    {
        menuHolder.SetActive(forSaving);

        // Text indicators for start and goal
        startText.gameObject.SetActive(!forSaving);
        goalText.gameObject.SetActive(!forSaving);

        apeScript.enabled = forSaving;

        gameCamera.gameObject.SetActive(forSaving);

        apeArrow.gameObject.SetActive(forSaving);
    }
}

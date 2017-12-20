using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour {
    int thisIndex;
    LevelSelect levelSelect;


	void Start () {
        DisableWindows();// Makes sure all windows are closed in the beginning
        levelSelect = GetComponentInParent<LevelSelect>();
	}

    public void EnableWindow(GameObject targetWindow)
    {
        if (!targetWindow.activeSelf)
        {
            DisableWindows();
        }
        else
        {
            targetWindow.SetActive(false);
            return;
        }


        if (targetWindow != null)
        {
            targetWindow.SetActive(true);
        }

        //EventSystem.current.SetSelectedGameObject(targetWindow.GetComponentInChildren<Button>().gameObject);
    }

    private void Update()
    {
        /*if (Input.GetButtonDown("Back"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponentInParent<MenuButtons>().DisableWindows();
        }*/
    }

    public void DisableWindows()// Disables all windows
    {
        if (thisIndex == 0)// Gets the index of this gameobject relative to the parent
        {
            for (int i = 0; i < transform.parent.childCount; i++)// Get the index of this gameobject
            {
                if (transform.parent.GetChild(i) != null && transform.parent.GetChild(i) == transform)
                {
                    thisIndex = i + 1;
                    break;
                }
            }
        }

        for (int i = thisIndex; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void StartLevel()
    {
        if(LevelSelect.lastIndexWithScore > 0)
            levelSelect.LoadScene(LevelSelect.lastIndexWithScore);
        else
        {
            levelSelect.LoadScene(0);
        }
    }

    public void LoadLevelByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

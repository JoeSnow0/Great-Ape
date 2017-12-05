using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour {
    int thisIndex;

	void Start () {
        DisableWindows();// Makes sure all windows are closed in the beginning
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
    }

    private void DisableWindows()// Disables all windows
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
}

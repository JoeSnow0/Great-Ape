using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class LogicLever : MonoBehaviour
{
    [SerializeField]
    bool on = false;

    // The image (like an 'E' or something) that pops up when the player can interact with the lever
    [SerializeField]
    GameObject indicatorObj;

    // Here you can add all the methods that will be called when the lever is triggered
    [SerializeField]
    UnityEvent leverMethods;

    bool m_canPress = false;

    PlayerInput m_currentApe;

    // The base of the stick on the lever 
    GameObject m_stickBase;

    private void Awake()
    {
        m_stickBase = transform.GetChild(0).gameObject;

        indicatorObj.SetActive(false);
    }


    private void Update()
    {
        m_stickBase.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(m_stickBase.transform.eulerAngles.z, (on) ? 330 : 390, 0.2f));

        if (!m_canPress)
            return;

        // If the ape that entered the collider is no longer in control, we remove the availability to use the lever
        if (!m_currentApe.GetApeState())
        {
            ToggleAvailability(false);
            return;
        }  

        //TODO: Fix correct keybinding for interaction
        if(Input.GetKeyDown(KeyCode.E))
        {
            Trigger();   
        }
    }

    public void Trigger ()
    {
        // Inverses the on-state
        on = !on;

        // Calls all the methods in the lever
        leverMethods.Invoke();
	}


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checks if the colliding object was a player
        if(other.CompareTag("Player"))
        {
            m_currentApe = other.GetComponent<PlayerInput>();
            // Checks if it was the player in control
            if (m_currentApe.GetApeState())
            {
                ToggleAvailability(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Checks if the collider exiting was the player that entered before
        if (other.gameObject == m_currentApe.gameObject)
        {
            ToggleAvailability(false);
        }
    }

    void ToggleAvailability(bool toggle)
    {
        indicatorObj.SetActive(toggle);
        m_canPress = toggle;
    }
}

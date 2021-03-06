﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class LogicLever : TriggerObject
{
    // The image (like an 'E' or something) that pops up when the player can interact with the lever
    [SerializeField]
    GameObject indicatorObj;

    bool m_canPress = false;

    PlayerInput m_currentApe;

    // The base of the stick on the lever 
    GameObject m_stickBase;

    private void Start()
    {
        if (on)
            triggerMethods.Invoke();
    }
    private void Awake()
    {
        m_stickBase = transform.GetChild(0).gameObject;

        indicatorObj.SetActive(false);

        m_stickBase.transform.Rotate(Vector3.forward, (on) ? -45 : 45);

        
    }


    private void Update()
    {
        if (!m_canPress)
        {
            if(m_currentApe != null)
            {
                if(m_currentApe.GetApeState())
                {
                    ToggleAvailability(true);
                }
            }
            return;
        }

        // If the ape that entered the collider is no longer in control, we remove the availability to use the lever
        if (!m_currentApe.GetApeState())
        {
            ToggleAvailability(false);
            return;
        }

        //TODO: Fix correct keybinding for interaction
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        // Inverses the on-state
        on = !on;

        // Rotates the lever based on the 'on'-state
        m_stickBase.transform.Rotate(Vector3.forward, (on) ? -90 : 90);

        // Calls all the methods in the lever
        triggerMethods.Invoke();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checks if the colliding object was a player
        if (other.CompareTag("Player"))
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
        if (m_currentApe == null)
            return;
        if (other.gameObject == m_currentApe.gameObject)
        {
            ToggleAvailability(false);
            m_currentApe = null;
        }
    }

    void ToggleAvailability(bool toggle)
    {
        indicatorObj.SetActive(toggle);
        m_canPress = toggle;
    }

    // TODO: properly interpolate z rotation to "animate" the lever
    IEnumerator ShiftLever(float rotAmount)
    {
        while (rotAmount != 0)
        {
            // Rotation amount this iteration
            float itRot = Mathf.Lerp(rotAmount, 0, 0.0001f);
            m_stickBase.transform.Rotate(Vector3.forward, itRot);
            rotAmount -= itRot;

            yield return 0;
        }
        yield return 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerObject : MonoBehaviour
{
    [SerializeField]
    public bool on = false;

    // Here you can add all the methods that will be called when the lever is triggered
    public UnityEvent triggerMethods;
}

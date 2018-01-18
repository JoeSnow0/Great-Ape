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


    private void OnDrawGizmos()
    {
        if (triggerMethods == null)
            return;

        Gizmos.color = new Color(1f, 0.6f, 0, 1);

        int i_max = triggerMethods.GetPersistentEventCount();
        for (int i = 0; i < i_max; i++)
        {
            //GameObject target = (triggerMethods.GetPersistentTarget(i) as Component).gameObject;

            //Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class AndTrigger : MonoBehaviour
{
    int requiredTriggers;
    int m_currentTriggers = 0;

    public List<TriggerObject> triggers = new List<TriggerObject>();

    public UnityEvent triggerMethod;

    bool on;

    void Awake()
    {
        requiredTriggers = triggers.Count;
        foreach(TriggerObject t in triggers)
        {
            t.triggerMethods.AddListener(() => Trigger(t));

            Trigger(t);
        }
    }

    private void Trigger(TriggerObject t)
    {
        m_currentTriggers += (t.on) ? 1 : 0;

        if (m_currentTriggers >= requiredTriggers)
        {
            on = true;

            triggerMethod.Invoke();
        }
        else if(on)
        {
            on = false;

            triggerMethod.Invoke();
        }
    }
}


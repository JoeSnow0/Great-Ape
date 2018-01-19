using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Vector2 openAmount = Vector2.zero;

    Vector3 orgPos;
    Vector3 goalPos;

    [SerializeField]
    [Range(0.01f, 1f)]
    float openScaleAmount = 1f;

    [SerializeField]
    bool open = false;

    bool m_isOpening = false;

    Vector3 currentGoal;

    void Start()
    {
        Vector3 origin = transform.position;
        Vector3 goal = transform.position + new Vector3(openAmount.x, openAmount.y, 0);

        if (open)
        {
            orgPos = goal;
            goalPos = origin;
        }
        else
        {
            goalPos = goal;
            orgPos = origin;
        }
    }


    private void FixedUpdate()
    {
        if (!m_isOpening)
            return;

        if (Vector3.Distance(transform.position, currentGoal) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, currentGoal, openScaleAmount);
        }
        else
        {
            m_isOpening = false;
        }
    }

    public void ToggleDoor()
    {
        // Bases the goal position based on if the door was open or not when we toggled it
        currentGoal = (open) ? orgPos : goalPos;

        open = !open;

        m_isOpening = true;
    }

    private IEnumerator ToggleAnimation(Vector3 goalPos)
    {
        m_isOpening = true;
        while (Vector3.Distance(transform.position, goalPos) > 1)
        {
            transform.position = Vector3.Lerp(transform.position, goalPos, openScaleAmount);
            yield return 0;
        }
        m_isOpening = false;

        yield return 0;
    }
}

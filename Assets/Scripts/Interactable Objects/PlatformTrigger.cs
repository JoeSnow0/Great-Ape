using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField]
    Vector2 goalMovement = Vector2.zero;

    [SerializeField]
    float speed;

    PlatformController m_platform;

    bool m_goalReached = false;
    bool m_moving = false;

    Vector3 m_orgPos;
    Vector3 m_goalPos;

    void Awake()
    {
        m_platform = gameObject.AddComponent<PlatformController>();

        m_platform.speed = 0;
        m_platform.localWaypoints = new Vector3[1] { transform.position };

        m_goalPos = m_orgPos = transform.position;
        m_goalPos += new Vector3(goalMovement.x, goalMovement.y, 0);
    }


    public void TogglePlatform()
    {
        if (m_moving)
            return;

        m_goalReached = !m_goalReached;

        Vector3 newGoalPos = (!m_goalReached) ? m_goalPos : m_orgPos;

        StartCoroutine(MovePlatform(newGoalPos));
    }


    IEnumerator MovePlatform(Vector3 goal)
    {
        m_platform.localWaypoints[0] = goal;
        m_platform.speed = speed;

        //while (Vector3.Distance(transform.position, )

        m_platform.speed = 0;

        yield return 0;
    }
}

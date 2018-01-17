using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeightedButton : TriggerObject
{
    [SerializeField]
    int requiredWeight = 1;

    int m_currentWeight = 0;

    [SerializeField]
    bool canTriggerOnce = false;

    [SerializeField]
    [Range(1, 10)]
    int rayAmount = 3;

    [SerializeField]
    float rayLength = 0.5f;

    [SerializeField]
    LayerMask mask;

    [SerializeField]
    TextMesh indicatorText;

    List<Ray2D> m_rays = new List<Ray2D>();

    List<GameObject> m_objectsOnButton = new List<GameObject>();

    // How far the button should be pressed when it's completely pressed down
    float m_buttonGoalHeight = 0;

    // Gets updated each frame to represent how much weight is on the button
    Vector3 newPos;

    void Awake ()
    {
        if (canTriggerOnce)
        {
            MeshRenderer rend = GetComponent<MeshRenderer>();
            rend.material.color = Color.green;
        }
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float width = bounds.size.x;
        // Calculates spacing between rays
        float spacing = width / (rayAmount - 1);

        int middleIndex = Mathf.CeilToInt(rayAmount / 2);
        float yRot = transform.eulerAngles.y;
        // Creates the specified amount of rays, spacing them apart
        for (int i = 0; i < rayAmount; i++)
        {
            Vector3 newOrigin = transform.position - Vector3.right * width / 2;
            newOrigin.x += i * spacing;
            Debug.Log(transform.eulerAngles.z / 360);
            newOrigin.y -= (middleIndex - i) * (transform.eulerAngles.z / 360);
            Debug.Log(newOrigin.y);

            m_rays.Add(new Ray2D(newOrigin, transform.rotation * Vector3.up));
        }

        // How far the button should be pressed down
        m_buttonGoalHeight = transform.localPosition.y - (transform.localScale.y * 0.7f);
	}
	
	void Update ()
    {
        UpdateRaycasts();

        float weightPercentage = Mathf.Min((m_currentWeight / requiredWeight), 1);
        //Debug.Log(weightPercentage);
        // Updates the text showcasing total weight on and needed for the button
        indicatorText.text = m_currentWeight + " / " + requiredWeight;
        indicatorText.color = new Color(1 - weightPercentage, weightPercentage, 0, 1);

        newPos = transform.localPosition;
        newPos.y = weightPercentage * m_buttonGoalHeight;
        transform.localPosition = newPos;
    }


    void UpdateRaycasts()
    {
        int totalWeight = 0;

        // Loops through all the rays created in Awake
        foreach(Ray2D ray in m_rays)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green, 0.1f);
            foreach(RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction, rayLength, mask))
            {
                GameObject current = hit.collider.gameObject;
                int weight = 0;
                if(current.layer == 8)
                {
                    if (!current.CompareTag("MovableObject"))
                        continue;
                    weight = (int)current.GetComponent<Rigidbody2D>().mass;
                }
                else
                {
                    weight = current.GetComponent<Player>().weight;
                }

                if (m_objectsOnButton.Find((x) => current == x))
                    continue;
                m_objectsOnButton.Add(current);

                totalWeight += weight;
            }
        }

        m_objectsOnButton.Clear();

        m_currentWeight = totalWeight;

        if (m_currentWeight < requiredWeight)
        {
            // If the button was on when the required weight was not met, that means the button will be deactivated
            if(on)
            {
                TriggerEvent();
            }
            return;
        }
        if (on)
            return;
        TriggerEvent();
    }

    private void TriggerEvent()
    {
        // Toggles 'on'-state
        on = !on;

        // Calls all the methods in the UnityEvent
        triggerMethods.Invoke();

        if (canTriggerOnce)
            enabled = false;
    }
}

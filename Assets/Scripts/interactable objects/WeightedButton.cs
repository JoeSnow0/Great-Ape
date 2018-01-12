using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeightedButton : MonoBehaviour
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

    [SerializeField]
    UnityEvent buttonMethods; 

    bool on = false;

    List<Ray2D> m_rays = new List<Ray2D>();

    List<GameObject> m_apesOnButton = new List<GameObject>();

    // How far the button should be pressed when it's completely pressed down
    float m_buttonGoalHeight = 0;

    // Gets updated each frame to represent how much weight is on the button
    Vector3 newPos;

    void Awake ()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float width = bounds.size.x;
        // Calculates spacing between rays
        float spacing = width / (rayAmount - 1);

        // Creates the specified amount of rays, spacing them apart
        for (int i = 0; i < rayAmount; i++)
        {
            Vector3 newOrigin = transform.position - Vector3.right * width / 2;
            newOrigin.x += i * spacing;

            m_rays.Add(new Ray2D(newOrigin, Vector3.up));
        }

        // How far the button should be pressed down
        m_buttonGoalHeight = transform.localPosition.y - (transform.localScale.y * 0.7f);
	}
	
	void Update ()
    {
        UpdateRaycasts();

        float weightPercentage = (m_currentWeight / requiredWeight);
        Debug.Log(weightPercentage);
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
                if (m_apesOnButton.Find((x) => hit.collider.gameObject == x))
                    continue;
                m_apesOnButton.Add(hit.collider.gameObject);

                totalWeight += hit.collider.GetComponent<Player>().weight;
            }
        }

        m_apesOnButton.Clear();

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
        buttonMethods.Invoke();

        if (canTriggerOnce)
            enabled = false;
    }
}

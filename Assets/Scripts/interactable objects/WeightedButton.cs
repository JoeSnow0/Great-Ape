using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : MonoBehaviour
{
    [SerializeField]
    int requiredWeight = 1;

    int currentWeight = 0;

    [SerializeField]
    [Range(1, 10)]
    int rayAmount = 3;

    [SerializeField]
    float rayLength = 0.5f;

    [SerializeField]
    LayerMask mask;

    bool on = false;

    List<Ray2D> rays = new List<Ray2D>();

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

            rays.Add(new Ray2D(newOrigin, Vector3.up));
        }
	}
	
	void Update ()
    {
        UpdateRaycasts();
	}


    void UpdateRaycasts()
    {
        int totalWeight = 0;
        // Loops through all the rays created in Awake
        foreach(Ray2D ray in rays)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green, 0.1f);
            foreach(RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction, rayLength, mask))
            {
                //totalWeight += hit.collider.GetComponent<Player>().weight;
            }
        }
    }
}

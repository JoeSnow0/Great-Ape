using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour {

    //Available Item
    public GameObject availableItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovableObject"))
        {
            availableItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == availableItem)
        {
            availableItem = null;
        }
    }
}

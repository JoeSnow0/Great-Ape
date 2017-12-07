using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BouncePadSettings : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks if player
        if (other.tag == "Player")
        {

            //if player reverce the speed of the player if the speed is downward
            if (other.GetComponent<PlayerConfig>().velocity.y <= 0)
                other.GetComponent<PlayerConfig>().velocity.y =
    -1 * other.GetComponent<PlayerConfig>().velocity.y;

        }
    }
}

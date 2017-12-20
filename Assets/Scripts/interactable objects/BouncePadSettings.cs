using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BouncePadSettings : MonoBehaviour
{
    public float bouncePower = 10;
    private void OnTriggerEnter2D(Collider2D other)
        
    {
        ////checks if player
        if (other.tag == "Player")
        {
            //if player reverce the speed of the player if the speed is downward
            other.GetComponent<PlayerConfig>().velocity.y = bouncePower;
        }
        else if(other.GetComponent<Rigidbody2D>() != null)
        {
            other.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bouncePower * 5, ForceMode2D.Force);
        }
    }
}

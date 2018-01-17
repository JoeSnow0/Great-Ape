using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BouncePadSettings : MonoBehaviour
{
    [SerializeField] float playerBouncePower = 200;
    [SerializeField] float otherBouncePower = 20;

    private void OnTriggerEnter2D(Collider2D other)
        
    {
        ////checks if player
        if (other.CompareTag("Player"))
        {
            //if player reverce the speed of the player if the speed is downward
            Player player = other.GetComponent<Player>();
            player.BouncePadJump(transform.rotation * Vector2.up * playerBouncePower);
        }
        else if(other.CompareTag("MovableObject"))
        {
            Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(transform.rotation * Vector2.up * otherBouncePower, ForceMode2D.Impulse);

            }
        }
    }
}

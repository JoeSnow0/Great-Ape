using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BouncePadSettings : MonoBehaviour
{
    [SerializeField] float playerBouncePower = 200;
    [SerializeField] float otherBouncePower = 20;
    public AudioClip bounceSFX;
    public float volMod = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
        
    {
        ////checks if player
        if (other.CompareTag("Player"))
        {
            //if player reverce the speed of the player if the speed is downward
            Player player = other.GetComponent<Player>();
            //Play sfx
            SoundManager.instance.PlayPitched(bounceSFX, 0.1f, 0.3f, volMod);
            //Add force upwards & account for rotation
            player.BouncePadJump(transform.rotation * Vector2.up * playerBouncePower);
        }
        else if(other.CompareTag("MovableObject"))
        {
            Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                //play sfx
                SoundManager.instance.PlayPitched(bounceSFX, 0.1f, 0.3f, volMod);
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                //Add force upwards & account for rotation
                rb2d.AddForce(transform.rotation * Vector2.up * otherBouncePower, ForceMode2D.Impulse);

            }
        }
    }
}

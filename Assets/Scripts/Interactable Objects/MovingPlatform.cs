using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public Vector2 velocity;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        rb2d.velocity = velocity;
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        velocity *= -1;
    }
}

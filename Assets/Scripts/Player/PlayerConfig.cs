using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerConfig : MonoBehaviour
{
    //Jumping Height
    [Range(1, 10)]
    public float jumpHeight = 4;
    //Jumping speed
    [Range (0, 1)]
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    //Movement
    float moveSpeed = 10;

    float gravity;
    float jumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        //stop gravity from accumulating over time when on the ground
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Jumping input
        if(Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
        //Different acceleration for running in air and on ground
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //Flip player sprite
        FlipPlayer();
    }
    void FlipPlayer()
    {

    }
}
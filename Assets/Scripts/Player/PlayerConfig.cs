using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerConfig : MonoBehaviour
{
    //Jumping Height
    [Range(1, 100)]
    public float maxJumpHeight = 4;
    [Range(1, 100)]
    public float minJumpHeight = 1;
    //Jumping speed
    [Range (0, 1)]
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    //Movement
    [Header("Movement"),Range(1, 100)]
    public float moveSpeed = 10;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;
    public bool isActive = false;
    
    PlayerController controller;
    public Animator apeAnimator;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        apeAnimator = GetComponentInChildren<Animator>();
        //Calculate gravity based on jump height and time to apex.
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

    }

    void Update()
    {
        //stop gravity from accumulating over time when on the ground
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        //Accept inputs
        Inputs();

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
    void FlipPlayer(float direction)
    {
        if (direction == 0)
            return;
        apeAnimator.transform.rotation = Quaternion.Euler(apeAnimator.transform.eulerAngles.x, (direction > 0) ? 0 : 180, apeAnimator.transform.eulerAngles.z);
    }

    void Inputs()
    {

        Vector2 input = Vector2.zero;
        if (isActive)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //Jumping input
            if (Input.GetButtonDown("Jump") && isActive)
            {
                print(controller.collisions.below);
                if(controller.collisions.below)
                {
                    velocity.y = maxJumpVelocity;
                    apeAnimator.Play("Jump");
                }
            }
        }

        //Check if player sprite needs to be flipped
        FlipPlayer(input.x);

        //Different acceleration for running in air and on ground
        float targetVelocityX = input.x * moveSpeed;
        
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        //If the player lets go of the jump button start falling earlier.
        if (Input.GetButtonUp("Jump") && isActive)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }
        //Animations
        if (input.x != 0 && controller.collisions.below)
        {
            apeAnimator.Play("Walking");
        }
        else if (controller.collisions.below)
        {
            apeAnimator.Play("Idle");
        }
    }
}
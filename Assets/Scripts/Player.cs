using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class Player : MonoBehaviour
{
    [Range(0, 100)]
    public float moveSpeed;

    [Header("Jump Variables")]
    [Range(0, 100)]
    public float maxJumpHeight = 4;
    [Range(0, 100)]
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    [Header("Wall Variables")]
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    [Header("Components")]

    public Controller2D controller;
    public PlayerInput playerInput;
    PlayerAnimation playerAnimation;
    public GameObject itemHolder;

    GameObject heldItem;

    [Header("Manager ref")]
    public ManagerConfig managerConfig;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    public Animator apeAnim;

    [Header("Ape Info")]
    [Range(0, 10)]
    public int weight = 0;

    public Gradient colorRange;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<Controller2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        managerConfig = FindObjectOfType<ManagerConfig>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        // Checks if we're currently not on the ground
        bool oldBelow = controller.collisions.below;

        // Sets animator values
        apeAnim.SetBool("Walking", (Mathf.Abs(velocity.x) > 2 && controller.collisions.below));
        apeAnim.SetBool("Falling", !controller.collisions.below && velocity.y < 0);
        apeAnim.SetBool("Grounded", controller.collisions.below);

        // Rotates the character mesh based on which way the player is facing
        if (velocity.x != 0)
            transform.GetChild(0).rotation = Quaternion.Euler(transform.eulerAngles.x, (velocity.x > 0) ? 0 : 180, transform.eulerAngles.z);


        CalculateVelocity();
        //HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }

        // If we just landed we trigger the landing animation
        if (controller.collisions.below && !oldBelow)
            apeAnim.SetTrigger("Land");

        
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }
    public void BouncePadJump(Vector2 bouncePower)
    {
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                velocity = bouncePower * (4f - weight);
            }
        }
    }
    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }


    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

    }

    public void TogglePickUpItem()
    {
        if(itemHolder != null)
        {
            if (heldItem != null)
            {
                heldItem.transform.SetParent(managerConfig.Mainground.transform);
                heldItem.GetComponent<Collider2D>().enabled = true;
                heldItem.GetComponent<Rigidbody2D>().simulated = true;
                heldItem.transform.position += Vector3.up * 5;
                heldItem = null;
            }

            //If hand is not empty
            GameObject item = itemHolder.GetComponentInChildren<PickupTrigger>().availableItem;
            if (item == null)
                return;

            itemHolder.GetComponentInChildren<PickupTrigger>().availableItem = null;
            //if hand is empty
            heldItem = item;
            heldItem.transform.SetParent(itemHolder.transform);
            heldItem.transform.localPosition = Vector3.zero;

            heldItem.GetComponent<Collider2D>().enabled = false;
            heldItem.GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}
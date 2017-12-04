using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerConfig : MonoBehaviour
{

    float moveSpeed = 6;
    float gravity = -20;
    Vector3 velocity;

    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
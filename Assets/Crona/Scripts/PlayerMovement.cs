using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References to character controller and camera transform
    public CharacterController myController;
    public Transform cam;

    // Jump and walk speed
    public float speed = 12f;

    // Simulate gravity
    private Vector3 velocity;
    private float gravity = -30f;
    public bool isGrounded;

    // Ground check variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Player Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Change movement direction according to camera direction
        if (direction.magnitude >= float.Epsilon)
        {
            float playerAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            Vector3 moveDirection = Quaternion.Euler(0f, playerAngle, 0f) * Vector3.forward;

            myController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        // Keep track of gravity
        velocity.y += gravity * Time.deltaTime;
        myController.Move(velocity * Time.deltaTime);

    }
}

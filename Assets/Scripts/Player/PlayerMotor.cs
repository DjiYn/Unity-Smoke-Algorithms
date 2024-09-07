using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGrounded;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float gravity = -9.8f;
    [SerializeField]
    private float jumpHeight = 3f;
    [SerializeField]
    private float runBoostSpeed = 3f;
    private bool isRunning;
    private float totalSpeed = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isRunning)
        {
            totalSpeed = speed + runBoostSpeed;
        } else
        {
            totalSpeed = speed;
        }

        characterController.Move(transform.TransformDirection(moveDirection) * totalSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0F * gravity);
        }
    }

    public void Run()
    {
        isRunning = !isRunning;
    }
}

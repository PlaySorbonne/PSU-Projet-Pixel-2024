using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class TestPlayerController : NetworkBehaviour {
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravityScale = 5f;
    public float maxFallSpeed = -100f;

    private float yVelocity = 0f;
    private CharacterController characterController; 

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Jump()
    {
        yVelocity = jumpForce;
    }

    // writing [Client(RequireOwnership = true)] above the Update method is the equivalent of doing "if (!IsOwner) return;" in the Update method
    void Update()
    {
        if (!IsOwner) return;


        yVelocity += Physics2D.gravity.y * Time.deltaTime * gravityScale;
        if (yVelocity < maxFallSpeed)
        {
            yVelocity = maxFallSpeed;
        }
        float horizontal = Input.GetAxis("Horizontal");
        
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            Jump();
        }

        Vector2 offset = new Vector2(horizontal * moveSpeed, yVelocity) * Time.deltaTime;

        characterController.Move(offset);
    }
}

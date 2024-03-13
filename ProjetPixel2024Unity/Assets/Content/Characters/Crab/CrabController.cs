using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : BasePlayerController
{
    private float yVelocity;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Jump()
    {
        yVelocity = jumpForce;
    }

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


        velocity = new Vector2(velocity.x, yVelocity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrabController : NetworkPlayerController
{
    private bool isGrounded;

    public void OnDash(InputValue value)
    {
        if (!IsOwner) return;

        //_dash = value.isPressed;
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("heyyyyyyyyyyyyyyyyyyyyyyyyyyyyy");
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;

        Vector2 movementInput = value.Get<Vector2>();
        xVelocity = movementInput.x * movementSpeed.x;
        yVelocity = Mathf.Min(0, yVelocity + movementInput.y * movementSpeed.y);  // don't jump but can slow down fall
    }

    public void OnAttack(InputValue value)
    {
        if ((!IsOwner) || (!value.isPressed)) return;

    }

    void OnJump()
    {
        yVelocity += jumpHeight;
    }

    void Update()
    {
        if (!IsOwner) return;

        isGrounded = characterController.isGrounded;

        yVelocity += Physics2D.gravity.y * Time.deltaTime * characterMassMultiplier ;
        if (yVelocity < maxFallSpeed)
        {
            yVelocity = maxFallSpeed;
        }

        if (isGrounded && yVelocity < 0)
        {
            yVelocity = 0f;
        }

        characterController.Move(new Vector2(xVelocity, yVelocity) * Time.deltaTime);
    }
}

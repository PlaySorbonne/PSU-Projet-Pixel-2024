using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquidController : NetworkPlayerController
{
    private bool isMovingUpwards = false;

     public void OnDash(InputValue value)
    {
        if (!IsOwner) return;

        //_dash = value.isPressed;
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;

        Vector2 movementInput = value.Get<Vector2>();
        xVelocity = movementInput.x * movementSpeed.x;
        yVelocity = movementInput.y * movementSpeed.y;
        isMovingUpwards = yVelocity > 0;
    }

    public void OnAttack(InputValue value)
    {
        if ((!IsOwner) || (!value.isPressed)) return;

    }

    void OnJump()
    {
       // yVelocity += jumpHeight;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (!isMovingUpwards) {
            yVelocity += Physics2D.gravity.y * Time.deltaTime * characterMassMultiplier ;
            if (yVelocity < maxFallSpeed)
            {
                yVelocity = maxFallSpeed;
            }
        }
        
        characterController.Move(new Vector2(xVelocity, yVelocity) * Time.deltaTime);
    }
}

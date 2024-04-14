using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrabController : NetworkPlayerController
{
    private bool isGrounded;
    public float maxJumpChargeTime = 1.5f;
    private float jumpChargeTime = 0.0f;
    private float lastJumpPower = 0.0f;

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
        yVelocity = Mathf.Max(maxFallSpeed, yVelocity + movementInput.y * movementSpeed.y);  // don't jump but can slow down fall
    }

    public void OnAttack(InputValue value)
    {
        if ((!IsOwner) || (!value.isPressed)) return;

    }

    void OnJump(InputValue value)
    {   
        // jump button pressed (holding the button charges the jump)
        if (value.isPressed) {
            jumpChargeTime = maxJumpChargeTime;
        }
        // jump button released
        else if (isGrounded) {
            jumpChargeTime = Mathf.Max(0.0f, jumpChargeTime);
            float jumpCharge = Mathf.Sqrt(1.0f - jumpChargeTime / maxJumpChargeTime);
            Debug.Log("Crab jump with charge=" + (jumpCharge*100f).ToString() + "% ; remainging time=" + jumpChargeTime.ToString());
            yVelocity += jumpHeight * jumpCharge;
            isGrounded = false;
            if (jumpCharge >= 0.95f) {
                lastJumpPower = 3.0f;
            } else {
                lastJumpPower = jumpCharge;
            }
        }
    }

    void JumpCrush()
    // Ability: crush opponents after powerful jump
    {
        cameraShake.ScreenShake(0.5f, 0.5f*lastJumpPower, 20f*lastJumpPower);
        Debug.Log("Crab crush with power=" + (lastJumpPower*100f).ToString() + "%");
        lastJumpPower = 0.0f;
        //TODO: add damage to nearby players
    }

    void Update()
    {
        if (!IsOwner) return;

        // check if character is on the ground
        isGrounded = characterController.isGrounded;

        // charge jump (only relevant if player is holding the jump button)
        jumpChargeTime -= Time.deltaTime;

        yVelocity += Physics2D.gravity.y * Time.deltaTime * characterMassMultiplier ;
        if (yVelocity < maxFallSpeed)
        {
            yVelocity = maxFallSpeed;
        }

        if (isGrounded)
        {
            if (yVelocity < 1.0f)
            {
                yVelocity = -1.0f;
                if (lastJumpPower > 0.0f) JumpCrush();
            }
        }

        characterController.Move(new Vector2(xVelocity, yVelocity) * Time.deltaTime);
    }
}

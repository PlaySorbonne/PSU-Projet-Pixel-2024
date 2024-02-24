using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Vertical movement variables
    [Header("Horizontal Movement")]
    public float movementSpeed = 10; //TODO : Try how it feels with force instead of velocity
    public float acceleration = 7;
    public float decceleration = 7;
    public float velPower = 0.9f;
    public float frictionAmount = 0.2f;
    #endregion

    #region Horizontal movement variables

    [Header("Vertical Movement")]
    public Transform groundCheck;
    public float groundCheckSize; 
    public float jumpForce = 15;
    
    #endregion

    #region inputs variables

    private float hInput;
    private bool vInput;
    private bool grounded;

    #endregion

    #region components variables

    private Rigidbody2D rb;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void FixedUpdate()
    {

        #region updating physics variables 

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckSize);

        #endregion

        #region horizontal movement

        //Applying horizontal force to move the player 

        float targetSpeed = hInput * movementSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
        
        //Applying force to simulate friction
        if (grounded && Mathf.Abs(hInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion



        #region vertical movement

        //TODOOOOOO UWU https://www.youtube.com/watch?v=KbtcEVCM7bw


        #endregion
    }

    public void OnMove(InputValue value)
    {
        hInput = value.Get<float>();
    }

    public void OnJump()
    {
        vInput = true; 
    }


}

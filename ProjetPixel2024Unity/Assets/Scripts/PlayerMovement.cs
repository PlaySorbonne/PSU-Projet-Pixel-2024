using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10; //TODO : Try how it feels with force instead of velocity
    public float jumpHeight = 10; 

    private Rigidbody2D rb;
    private Vector2 movementDirection;

    private float f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }


    private void Update()
    {
        //Debug.Log(f);
    }

    private void FixedUpdate()
    {
        //rb.AddForce(movementDirection * new Vector2(movementSpeed, jumpHeight));
    }

    public void OnMove(InputValue value)
    {
        //movementDirection.x = value.Get<float>();
        rb.velocity= new Vector2(value.Get<float>() * movementSpeed, rb.velocity.y);
    }

    public void OnJump()
    {
        //movementDirection.y = 1;
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Object.Prediction;

public class BasePlayerController : NetworkBehaviour {
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravityScale = 1f;
    public float maxFallSpeed = -100f;

    protected Vector2 velocity = Vector2.zero;
    protected CharacterController characterController;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
}

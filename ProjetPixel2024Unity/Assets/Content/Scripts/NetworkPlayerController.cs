using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FishNet;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerController : NetworkBehaviour
{
    public GameObject attackHitboxPrefab;
    
    [Header("Gameplay variables")]
    public int health = 5;
    public float characterMassMultiplier = 1.0f;
    public float jumpHeight = 40f;
    public float friction = 0.5f;
    public Vector2 movementSpeed = new Vector2(10f, 3f);
    public float maxFallSpeed = -100f;

    protected float xVelocity = 0f;
    protected float yVelocity = 0f;
    protected CharacterController characterController;    
    protected Camera playerCamera;
    protected Shake cameraShake;

    [Header("Input variables")]
    [SerializeField]
    protected InputActionAsset inputActionAsset;


    [Header("Others")]
    [SerializeField]
    protected Vector2 direction = new Vector2(1,0);

    protected Vector2 _move;

    public override void OnStartNetwork()
    {
        characterController = GetComponent<CharacterController>();
        // find first object with tag MainCamera
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraShake = playerCamera.GetComponent<Shake>();
    }

    public override void OnStartClient()
    {
        if (!IsOwner) return;
    }
    
}

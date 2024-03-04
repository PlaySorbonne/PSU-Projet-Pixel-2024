using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : NetworkBehaviour
{
    [SerializeField]
    private InputActionAsset inputActionAsset;

    private InputAction _moveAction;

    private InputAction _jumpAction;

    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float movementSpeed = 10f;

    [SerializeField]
    private float jumpHeight = 40f;

    [SerializeField]
    private Vector2 direction = new Vector2(1,0);

    private float _move;

    private bool _jump;

    public override void OnStartNetwork()
    {
        _moveAction = inputActionAsset.FindAction("Move");
        _jumpAction = inputActionAsset.FindAction("Jump");
        _moveAction.Enable();
        _jumpAction.Enable();
        _rigidbody = GetComponent<Rigidbody2D>();
        TimeManager.OnTick += TimeManagerTickEventHandler;
        TimeManager.OnPostTick += TimeManagerPostTickEventHandler;
    }

    public override void OnStopNetwork() 
    {
        TimeManager.OnTick -= TimeManagerTickEventHandler;
        TimeManager.OnPostTick -= TimeManagerPostTickEventHandler;
    }

    public override void OnStartClient()
    {
        if (!IsOwner) return;
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (!IsOwner) return;

    //     _move = _moveAction.ReadValue<float>();
    // }

    public void OnJump(InputValue value)
    {
        if (!IsOwner) return;

        _jump = value.isPressed;
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;

        _move = value.Get<float>();
    }

    private void TimeManagerTickEventHandler()
    {
        if (IsOwner)
        {
            MovementData md = new(_move, _jump);
            _jump = false;
            Replicate(md);
        }
        else
        {
            Replicate(default(MovementData));
        }
    }

    private void TimeManagerPostTickEventHandler()
    {
        if (!IsServerStarted) return;

        ReconciliationData rd = new(_rigidbody.position.x, _rigidbody.velocity);

        Reconcile(rd);
    }

    [Replicate]
    private void Replicate(MovementData md, ReplicateState rs = ReplicateState.Invalid, Channel channel = Channel.Unreliable)
    {
        if (md.Jump)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpHeight);
        }
        if (md.Move != 0.0f) 
        {
            direction.x = 1.0f * Mathf.Sign(md.Move);
        }
        _rigidbody.velocity = new Vector2(md.Move * movementSpeed, _rigidbody.velocity.y);
    }

    [Reconcile]
    private void Reconcile(ReconciliationData rd, Channel channel = Channel.Unreliable)
    {
        //_rigidbody.position.x = rd.PositionX;
        _rigidbody.MovePosition(new Vector2(rd.PositionX, _rigidbody.position.y));
        _rigidbody.velocity = rd.Velocity;
    }

    private struct MovementData : IReplicateData
    {
        private uint _tick;
        public readonly float Move;
        public readonly bool Jump;

        public MovementData(float move, bool jump)
        {
            _tick = 0u;
            Move = move;
            Jump = jump;
        }

        public readonly uint GetTick()
        {
            return _tick;
        }

        public void SetTick(uint value)
        {
            _tick = value;
        }

        public void Dispose() { }
    }

    private struct ReconciliationData : IReconcileData
    {
        private uint _tick;
        public float PositionX;
        public Vector2 Velocity;

        public ReconciliationData(float positionX, Vector2 velocity)
        {
            _tick = 0;
            PositionX = positionX;
            Velocity = velocity;
        }

        public readonly uint GetTick()
        {
            return _tick;
        }

        public void SetTick(uint value)
        {
            _tick = value;
        }

        public void Dispose() { }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerController : NetworkBehaviour
{
    [Header("Gameplay variables")]
    public int health = 5;
    public float characterMassMultiplier = 1.0f;
    public Vector2 movementSpeed = new Vector2(10f, 3f);
    public float jumpHeight = 40f;  // might remove this variable cuz it's not common to every animal
    public float friction = 0.5f;
    

    [Header("Input variables")]
    [SerializeField]
    protected InputActionAsset inputActionAsset;

    protected InputAction _moveAction;

    protected InputAction _attackAction;
    protected InputAction _defenseAction;
    protected InputAction _dashAction;

    protected Rigidbody2D _rigidbody;


    [Header("Others")]
    [SerializeField]
    protected Vector2 direction = new Vector2(1,0);

    protected Vector2 _move;

    protected bool _dash;
    protected bool _attack;
    protected bool _defense;

    public override void OnStartNetwork()
    {
        _moveAction = inputActionAsset.FindAction("Move");
        _dashAction = inputActionAsset.FindAction("Dash");
        _attackAction = inputActionAsset.FindAction("Attack");
        _defenseAction = inputActionAsset.FindAction("Defense");
        _attackAction.Enable();
        _defenseAction.Enable();
        _moveAction.Enable();
        _dashAction.Enable();
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

    public void OnDash(InputValue value)
    {
        if (!IsOwner) return;

        _dash = value.isPressed;
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;

        _move = value.Get<Vector2>();
    }

    protected void TimeManagerTickEventHandler()
    {
        if (IsOwner)
        {
            MovementData md = new(_move, _dash);
            _dash = false;
            Replicate(md);
        }
        else
        {
            Replicate(default(MovementData));
        }
    }

    protected void TimeManagerPostTickEventHandler()
    {
        if (!IsServerStarted) return;

        ReconciliationData rd = new(_rigidbody.position, _rigidbody.velocity);

        Reconcile(rd);
    }

    [Replicate]
    private void Replicate(MovementData md, ReplicateState rs = ReplicateState.Invalid, Channel channel = Channel.Unreliable)
    {
        if (md.Dash)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpHeight);
        }
        if (md.Move.x != 0.0f) 
        {
            direction.x = 1.0f * Mathf.Sign(md.Move.x);
        }
        _rigidbody.velocity = new Vector2(md.Move.x * movementSpeed.x, _rigidbody.velocity.y);
    }

    [Reconcile]
    private void Reconcile(ReconciliationData rd, Channel channel = Channel.Unreliable)
    {
        _rigidbody.MovePosition(rd.Position);
        _rigidbody.velocity = rd.Velocity;
    }

    private struct MovementData : IReplicateData
    {
        private uint _tick;
        public readonly Vector2 Move;
        public readonly bool Dash;

        public MovementData(Vector2 move, bool dash)
        {
            _tick = 0u;
            Move = move;
            Dash = dash;
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
        public Vector3 Position;
        public Vector2 Velocity;

        public ReconciliationData(Vector3 position, Vector2 velocity)
        {
            _tick = 0;
            Position = position;
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

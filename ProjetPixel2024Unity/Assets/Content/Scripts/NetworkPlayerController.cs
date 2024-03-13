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
    public Vector2 movementSpeed = new Vector2(10f, 3f);
    public float jumpHeight = 40f;  // might remove this variable cuz it's not common to every animal
    public float friction = 0.5f;
    

    [Header("Input variables")]
    [SerializeField]
    protected InputActionAsset inputActionAsset;
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

    public void OnAttack(InputValue value)
    {
        if (!IsOwner) return;

        _attack = value.isPressed;   
    }

    protected void TimeManagerTickEventHandler()
    {
        if (IsOwner)
        {
            MovementData md = new(_move.x, _move.y, _dash, _attack);
            _dash = false;
            _attack = false;
            Replicate(md);
        }
        else
        {
            //Replicate(default(MovementData));
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
        if (md.Attack) 
        {
            GameObject attackHitbox = Instantiate(attackHitboxPrefab, transform.position, Quaternion.identity);
            attackHitbox.transform.position = transform.position + direction.ConvertTo<Vector3>();
            InstanceFinder.ServerManager.Spawn(attackHitbox, null);
        }
        if (md.MoveX != 0.0f) 
        {
            direction.x = 1.0f * Mathf.Sign(md.MoveX);
        }

        _rigidbody.velocity = new Vector2(md.MoveX * movementSpeed.x, _rigidbody.velocity.y);
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
        public readonly float MoveX;
        public readonly float MoveY;
        public readonly bool Dash;

        public readonly bool Attack;

        public MovementData(float moveX, float moveY, bool dash, bool attack)
        {
            _tick = 0u;
            MoveX = moveX;
            MoveY = moveY;
            Dash = dash;
            Attack = attack;
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

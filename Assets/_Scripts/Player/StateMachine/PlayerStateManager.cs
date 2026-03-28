using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{    
    [SerializeField] private InputReader input;
    
    // --- Component References ---
    public Rigidbody2D _rb;
    public GameObject _spriteGO;
    public Collider2D _feetColl;
    public GroundCheck _groundCheck;


    // --- States ---
    public PlayerIdleState idleState {get; private set; }
    public PlayerMoveState moveState {get; private set; }    
    public PlayerJumpState jumpState {get; private set; }

    private PlayerBaseState _currentState;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        idleState = new PlayerIdleState(this,input);
        moveState = new PlayerMoveState(this,input);
        jumpState = new PlayerJumpState(this,input);
    }

    void Start()
    {
        _currentState = idleState;
        _currentState.EnterState();
    }

    
    void Update()
    {
        _currentState.UpdateState();
    }
    void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    public void SwitchStateTo(PlayerBaseState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        newState.EnterState();
    }
    void OnEnable()
    {
         
    }
}

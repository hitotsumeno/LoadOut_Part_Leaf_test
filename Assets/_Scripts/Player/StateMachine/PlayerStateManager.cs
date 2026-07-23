using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{    
    [SerializeField] private InputReader input;
    public PlayerMoveStatsSCO MoveStats;
    
    // --- Component References ---
    [Header("--- Component References ---")]
    public Rigidbody2D _rb;
    public GameObject _spriteGO;
    public Collider2D _feetColl;
    public GroundCheck _groundCheck;


    // --- States ---
    [Header("--- States ---")]
    public PlayerIdleState idleState;
    public PlayerMoveState moveState; 
    public PlayerJumpState jumpState;

    private PlayerBaseState _currentState;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        idleState.Init(this,input);
        moveState.Init(this,input);
        jumpState.Init(this,input);
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

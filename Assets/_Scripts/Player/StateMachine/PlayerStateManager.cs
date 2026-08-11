using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
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

    public float p_HorizontalVelocity;
    public float p_VerticalVelocity;


    // --- States ---
    [Header("--- States ---")]
    public PlayerIdleState idleState;
    public PlayerMoveState moveState; 
    public PlayerAirState airState;

    private PlayerBaseState _currentState;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        idleState.Init(this,input);
        moveState.Init(this,input);
        airState.Init(this,input);
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
        ApplyVelocity();
    }

    public void SwitchStateTo(PlayerBaseState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        newState.EnterState();
    }
    
    private void ApplyVelocity()
    {        
        _rb.velocity = new Vector2(p_HorizontalVelocity, p_VerticalVelocity);
    }
}

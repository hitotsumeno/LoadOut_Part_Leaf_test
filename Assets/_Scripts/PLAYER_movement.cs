using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_movement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private GameObject _spriteGO;
    [SerializeField] private Collider2D _feetColl;
    
    private Vector2 _movementDir;
    private Vector2 _moveVelocity;

    [SerializeField] private float VerticalForce;
    [SerializeField] private float HorizontalSpeed;
    
    // MoveStats
    [SerializeField] private float groundAcceleration = 5f;
    [SerializeField] private float groundDeceleration = 20f;
    [SerializeField] private float horizontalMaxSpeed = 12.5f;
    
    //Collision check var
    private RaycastHit2D _groundHit;
    [SerializeField]private bool _isGrounded;
    [SerializeField] private float groundDetectionRayLength;

    [SerializeField] private LayerMask groundLayer;

    [Header("Debug")]
    [SerializeField] private bool debugShowIsGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            _movementDir.x = Input.GetAxis("Horizontal");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        //RotateCircle();
    }


    private void FixedUpdate()
    {
        CollisionChecks();

        Move(groundAcceleration, groundDeceleration, _movementDir);
        RotateCircle();
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, 0) * horizontalMaxSpeed;

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.deltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }

        if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity,Vector2.zero, deceleration * Time.deltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
    }
    private void Jump()
    {
        if (_isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, VerticalForce);
        }
    }

    private void RotateCircle()
    {
        
    }

    #region Ground Check
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.center.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, groundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, groundDetectionRayLength, groundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        #region Debug Visualization
        if (debugShowIsGrounded)
        {
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - groundDetectionRayLength), Vector2.right * boxCastOrigin, rayColor);
        }
        #endregion
    }

    private void CollisionChecks()
    {
        IsGrounded();
    }

    #endregion
}

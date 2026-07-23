using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    [SerializeField] private float VerticalForce = 9f;

    // Jump Event var
    bool JumpWasPressed;
    bool JumpWasReleased;
    bool JumpIsHeld;

    // Jump var
    public float VerticalVelocity {get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpUsed;

    // Apex var
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    // Jump buffer var
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    // Coyote Time var

    private float _coyoteTimer;

#region Enter/Exit State logic
    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpPressedEvent += HandleJumpPressed;
        Reader.JumpIsHeldEvent += HandleJumpIsHeld;
        Reader.JumpReleaseEvent += HandleJumpRelease;
        JumpPressed();
        JumpChecks();
        Jump();
        Debug.Log("Player enters Jump state");
    }

    public override void ExitState()
    {
        Reader.MoveEvent -= HandleMove;
        Reader.JumpPressedEvent -= HandleJumpPressed;
        Reader.JumpIsHeldEvent -= HandleJumpIsHeld;
        Reader.JumpReleaseEvent -= HandleJumpRelease;
    }
#endregion

    public override void UpdateState()
    {
        JumpChecks();
        CountTimers();
    }
    public override void FixedUpdateState()
    {
        Jump();
    }

    private void JumpChecks()
    {

        //INITIATE JUMP WITH JUMP BUFFERS AND COYOTE TIME
        if (_jumpBufferTimer > 0f && !_isJumping && (StateManager._groundCheck._isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }

        //DOUBLE JUMP
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpUsed < StateManager.MoveStats.NumberOfJumpAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }

        //AIR JUMP AFTER COYOTE TIME LAPSED
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpUsed < StateManager.MoveStats.NumberOfJumpAllowed - 1)
        {
            InitiateJump(2);
            _isFastFalling = false;

        }

        //LANDED
        if((_isJumping || _isFalling) && StateManager._groundCheck._isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _isPastApexThreshold = false;
            _fastFallTime = 0f;
            _numberOfJumpUsed = 0;
            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumpUsed)
    {
        //apply soundFX for the jump
        // SoundFXManager.Instance.PlaySoundFXClip(JumpAudioClip, transform, 0.5f);

        if(!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpUsed += numberOfJumpUsed;
        VerticalVelocity = StateManager.MoveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        //APPLY GRAVITY WHILE JUMPING
        if (_isJumping)
        {
            //CHECK FOR HEAD BUMP
            // if (_bumpedHead)
            // {
            //     _isFastFalling = true;
            // }

            //GRAVITY ON ASCENDING
            if (VerticalVelocity >= 0f)
            {
                //APEX CONTROLS
                _apexPoint = Mathf.InverseLerp(StateManager.MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > StateManager.MoveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < StateManager.MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else { VerticalVelocity = -0.01f; }
                    }
                }

                //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
                else
                {
                    VerticalVelocity += StateManager.MoveStats.Gravity * Time.deltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }

            //GRAVITY ON DESCENDING
            else if (!_isFastFalling)
            {
                VerticalVelocity += StateManager.MoveStats.Gravity * StateManager.MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        //JUMP CUT
        if (_isFastFalling)
        {
            if (_fastFallTime >= StateManager.MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += StateManager.MoveStats.Gravity * StateManager.MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < StateManager.MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / StateManager.MoveStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime; 
        }

        //NORMAL GRAVITY WHILE FALLING
        if (!StateManager._groundCheck._isGrounded && !_isJumping) 
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += StateManager.MoveStats.Gravity * Time.fixedDeltaTime;
        }

        //CLAMP FALL SPEED 
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -StateManager.MoveStats.MaxFallSpeed, 50f);
        
        //Apply all effects on the vector Y velocity
        StateManager._rb.velocity = new Vector2(StateManager._rb.velocity.x, VerticalVelocity);

        // if (StateManager._groundCheck._isGrounded)
        // {
        //     StateManager._rb.velocity = new Vector2(StateManager._rb.velocity.x, VerticalForce);
        // }
    }

   #region Timers
    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if(!StateManager._groundCheck._isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else
        {
            _coyoteTimer = StateManager.MoveStats.JumpCoyoteTime;
        }
    }
    #endregion

    private void JumpPressed()
    {
        _jumpBufferTimer = StateManager.MoveStats.JumpBufferTime;
        _jumpReleasedDuringBuffer = false;
        
        // Jump();
    }

    private void JumpReleased()
    {
        if (_jumpBufferTimer > 0f)
        {
            _jumpReleasedDuringBuffer = true;
        }

        if(_isJumping && VerticalVelocity > 0f)
        {
            if (_isPastApexThreshold)
            {
                _isPastApexThreshold = false;
                _isFastFalling = true;
                _fastFallTime = StateManager.MoveStats.TimeForUpwardsCancel;
                VerticalVelocity = 0f;
            }
            else
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }   
    }


    #region Handle Events
    private void HandleJumpPressed()
    {
        // When we pressed the jump button
        JumpPressed();
    }

    private void HandleJumpRelease()
    {
        //When we release  the jump button
       JumpReleased();
    }

    private void HandleJumpIsHeld()
    {
        // throw new NotImplementedException();
    }
    private void HandleMove(float dir)
    {
        if (dir != 0f)
        {
            StateManager.SwitchStateTo(StateManager.moveState);
        } 
        else 
            return;
    }
    #endregion
}

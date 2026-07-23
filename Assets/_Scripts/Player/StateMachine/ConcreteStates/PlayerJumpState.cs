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




    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpPressedEvent += HandleJumpPressed;
        Reader.JumpIsHeldEvent += HandleJumpIsHeld;
        Reader.JumpReleaseEvent += HandleJumpRelease;
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

    public override void FixedUpdateState()
    {
        Jump();
    }

    public override void UpdateState()
    {
        // JumpChecks();
    }

    // private void JumpChecks()
    // {

    //     //INITIATE JUMP WITH JUMP BUFFERS AND COYOTE TIME
    //     if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
    //     {
    //         InitiateJump(1);

    //         if (_jumpReleasedDuringBuffer)
    //         {
    //             _isFastFalling = true;
    //             _fastFallReleaseSpeed = VerticalVelocity;
    //         }
    //     }

    //     //DOUBLE JUMP
    //     else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpUsed < MoveStats.NumberOfJumpAllowed)
    //     {
    //         _isFastFalling = false;
    //         InitiateJump(1);
    //     }

    //     //AIR JUMP AFTER COYOTE TIME LAPSED
    //     else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpUsed < MoveStats.NumberOfJumpAllowed - 1)
    //     {
    //         InitiateJump(2);
    //         _isFastFalling = false;

    //     }

    //     //LANDED
    //     if((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
    //     {
    //         _isJumping = false;
    //         _isFalling = false;
    //         _isFastFalling = false;
    //         _isPastApexThreshold = false;
    //         _fastFallTime = 0f;
    //         _numberOfJumpUsed = 0;
    //         VerticalVelocity = Physics2D.gravity.y;
    //     }
    // }

    // private void InitiateJump(int numberOfJumpUsed)
    // {
    //     //apply soundFX for the jump
    //     // SoundFXManager.Instance.PlaySoundFXClip(JumpAudioClip, transform, 0.5f);

    //     if(!_isJumping)
    //     {
    //         _isJumping = true;
    //     }

    //     _jumpBufferTimer = 0f;
    //     _numberOfJumpUsed += numberOfJumpUsed;
    //     VerticalVelocity = MoveStats.InitialJumpVelocity;
    // }

    private void Jump()
    {
        // //APPLY GRAVITY WHILE JUMPING
        // if (_isJumping)
        // {
        //     //CHECK FOR HEAD BUMP
        //     if (_bumpedHead)
        //     {
        //         _isFastFalling = true;
        //     }

        //     //GRAVITY ON ASCENDING
        //     if (VerticalVelocity >= 0f)
        //     {
        //         //APEX CONTROLS
        //         _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

        //         if (_apexPoint > MoveStats.ApexThreshold)
        //         {
        //             if (!_isPastApexThreshold)
        //             {
        //                 _isPastApexThreshold = true;
        //                 _timePastApexThreshold = 0f;
        //             }

        //             if (_isPastApexThreshold)
        //             {
        //                 _timePastApexThreshold += Time.fixedDeltaTime;
        //                 if (_timePastApexThreshold < MoveStats.ApexHangTime)
        //                 {
        //                     VerticalVelocity = 0f;
        //                 }
        //                 else { VerticalVelocity = -0.01f; }
        //             }
        //         }

        //         //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
        //         else
        //         {
        //             VerticalVelocity += MoveStats.Gravity * Time.deltaTime;
        //             if (_isPastApexThreshold)
        //             {
        //                 _isPastApexThreshold = false;
        //             }
        //         }
        //     }

        //     //GRAVITY ON DESCENDING
        //     else if (!_isFastFalling)
        //     {
        //         VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
        //     }

        //     else if (VerticalVelocity < 0f)
        //     {
        //         if (!_isFalling)
        //         {
        //             _isFalling = true;
        //         }
        //     }
        // }

        // //JUMP CUT
        // if (_isFastFalling)
        // {
        //     if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
        //     {
        //         VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
        //     }
        //     else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
        //     {
        //         VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
        //     }

        //     _fastFallTime += Time.fixedDeltaTime; 
        // }

        // //NORMAL GRAVITY WHILE FALLING
        // if (!_isGrounded && !_isJumping) 
        // {
        //     if (!_isFalling)
        //     {
        //         _isFalling = true;
        //     }

        //     VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        // }

        // //CLAMP FALL SPEED 
        // VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);
        
        // //Apply all effects on the vector Y velocity
        // StateManager._rb.velocity = new Vector2(StateManager._rb.velocity.x, VerticalVelocity);

        if (StateManager._groundCheck._isGrounded)
        {
            StateManager._rb.velocity = new Vector2(StateManager._rb.velocity.x, VerticalForce);
        }
    }

   #region Timers
    // private void CountTimers()
    // {
    //     _jumpBufferTimer -= Time.deltaTime;

    //     if(!StateManager._groundCheck._isGrounded)
    //     {
    //         _coyoteTimer -= Time.deltaTime;
    //     }
    //     else
    //     {
    //         _coyoteTimer = MoveStats.JumpCoyoteTime;
    //     }
    // }
    #endregion

    #region Handle Events
    private void HandleJumpPressed()
    {

        // _jumpBufferTimer = MoveStats.JumpBufferTime;
        // _jumpReleasedDuringBuffer = false;
        
        Jump();
    }

    private void HandleJumpRelease()
    {
        // //When we release  the jump button
        // if (JumpWasReleased)
        // {
        //     if (_jumpBufferTimer > 0f)
        //     {
        //         _jumpReleasedDuringBuffer = true;
        //     }

        //     if(_isJumping && VerticalVelocity > 0f)
        //     {
        //         if (_isPastApexThreshold)
        //         {
        //             _isPastApexThreshold = false;
        //             _isFastFalling = true;
        //             _fastFallTime = MoveStats.TimeForUpwardsCancel;
        //             VerticalVelocity = 0f;
        //         }
        //         else
        //         {
        //             _isFastFalling = true;
        //             _fastFallReleaseSpeed = VerticalVelocity;
        //         }
        //     }
        // }

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

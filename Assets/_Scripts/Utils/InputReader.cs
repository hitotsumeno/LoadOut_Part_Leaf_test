using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName ="Scriptableobjects/Input Reader")]
public class InputReader : ScriptableObject, InputMap.IPlayerControlsActions, InputMap.IUIControlsActions
{
    private InputMap _input;

#region Enable/Disable
    void OnEnable()
    {
        if (_input == null)
        {
            _input = new InputMap();

            _input.PlayerControls.SetCallbacks(this);
            _input.UIControls.SetCallbacks(this);

            EnableGameplayInput();
        }
        _input.Enable();
    }

    void OnDisable()
    {
        DisableAllInput();
        _input.Disable();
    }

    private void EnableGameplayInput()
    {
        _input.PlayerControls.Enable();
        _input.UIControls.Disable();
    }

    private void EnableUIInput()
    {
        _input.PlayerControls.Disable();
        _input.UIControls.Enable();
    }

    private void DisableAllInput()
    {
        _input.PlayerControls.Disable();
        _input.UIControls.Disable();
    }
#endregion

    public event Action<float> MoveEvent;
    public event Action InteractEvent;
    public event Action JumpPressedEvent;
    public event Action JumpIsHeldEvent;
    public event Action JumpReleaseEvent;
    public event Action PauseEvent;
    public event Action ResumeEvent;
    
#region Event
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<float>());
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed )
        {
            InteractEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            JumpPressedEvent?.Invoke();
            // Debug.Log("Jump button was pressed");
        }

        if (context.phase == InputActionPhase.Performed)
        {
            JumpIsHeldEvent?.Invoke();
            // Debug.Log("Jump button is Held");
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            JumpReleaseEvent?.Invoke();
            // Debug.Log("Jump button was release");
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            EnableUIInput();
            PauseEvent?.Invoke();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            EnableGameplayInput();
            ResumeEvent?.Invoke();
        }
    }
    #endregion
}
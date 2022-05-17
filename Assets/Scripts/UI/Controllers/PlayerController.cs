using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController<JoyStickView>
{
    public Action<Vector3> OnDirectionChanged;
    public Action OnMoveStarted;
    public Action OnMoveStopped;
    private bool _pressed;
    private bool _lockInput;

    private void Awake()
    {
        GameManager.PlayerController = this;
    }

    private void Update()
    {
        if(_lockInput)
            return;
        TouchClickInputProcess();
        TouchMoveInputProcess();
    }

    public void LockInput(bool lockInput)
    {
        _lockInput = lockInput;
    }
    
    private void TouchClickInputProcess()
    {
        if (Input.GetMouseButtonDown(0))
            TouchDown();
        else if (Input.GetMouseButtonUp(0))
            TouchUp();
    }

    private void TouchDown()
    {
        _pressed = true;
        ui.SetControllerPosition(Input.mousePosition);
        OnMoveStarted?.Invoke();
    }

    private void TouchUp()
    {
        _pressed = false;
        ui.Hide();
        OnMoveStopped?.Invoke();
    }
    
    private void TouchMoveInputProcess()
    {
        if(_pressed == false)
            return;
        var direction = ui.UpdateInnerPosition(Input.mousePosition);
        OnDirectionChanged?.Invoke(direction);
    }
}

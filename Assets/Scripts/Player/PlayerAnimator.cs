using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimator : BodyAnimator
{
    private const string RunAnimation = "Run";
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string WinAnimation = "Win";

    public override void SetIdle()
    {
        _animator.SetBool(RunAnimation, false);
        base.SetIdle();
    }
    
    public void SetRun()
    {
        _animator.SetBool(IdleAnimation, false);
        _animator.SetBool(RunAnimation, true);
        //TriggerAnimate(RunAnimation);
    }

    public void SetWin()
    {
        _animator.SetBool(IdleAnimation, false);
        _animator.SetBool(RunAnimation, false);
        _animator.SetBool(WinAnimation, true);
    }
    
    public void SetRunDirection(Vector3 direction)
    {
        _animator.SetFloat(Horizontal, direction.x);
        _animator.SetFloat(Vertical, direction.z);
    }

    /*private void TriggerAnimate(string triggerName)
    {
        //DisableOtherAnimations(triggerName);
        _animator.SetTrigger(triggerName);
    }*/

    /*private void DisableOtherAnimations(string animationName)
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.name != animationName) // && parameter.GetType() == typeof(bool)
            {
                _animator.SetBool(parameter.name, false);
            }
        }
    }

    private void DisableAllAnimations()
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            _animator.SetBool(parameter.name, false);
        }
    }*/
}

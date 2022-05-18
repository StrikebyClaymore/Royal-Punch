using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    private readonly int _horizontal = Animator.StringToHash("Horizontal");
    private readonly int _vertical = Animator.StringToHash("Vertical");
    private readonly int _move = Animator.StringToHash("Move");
    private readonly int _win = Animator.StringToHash("Win");

    public void StartMove() => animator.SetBool(_move, true);

    public void StopMove() => animator.SetBool(_move, false);

    public void StartWin() => animator.Play(_win, 0);
    
    public void SetDirection(float horizontal, float vertical)
    {
        animator.SetFloat(_horizontal, horizontal);
        animator.SetFloat(_vertical, vertical);
    }
}

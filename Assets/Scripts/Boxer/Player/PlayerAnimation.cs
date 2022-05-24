using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    private readonly int _horizontal = Animator.StringToHash("Horizontal");
    private readonly int _vertical = Animator.StringToHash("Vertical");
    private readonly int _move = Animator.StringToHash("Move");
    private readonly int _flexIdle = Animator.StringToHash("FlexIdle");
    private readonly int _win = Animator.StringToHash("Win");
    private readonly int _lose = Animator.StringToHash("Lose");

    public void PlayStartAnim()
    {
        if(GameManager.Win || GameManager.FirstTry)
            StartFlexIdle();
        else
            StartLoseIdle();
    }
    
    public void StartMove() => animator.SetBool(_move, true);

    public void StopMove() => animator.SetBool(_move, false);

    public void StartWin() => animator.Play(_win, 0);
    
    public void StartLoseIdle() => animator.Play(_lose, 0);
    
    public void StartFlexIdle() => animator.Play(_flexIdle, 0);
    
    public void SetDirection(float horizontal, float vertical)
    {
        animator.SetFloat(_horizontal, horizontal);
        animator.SetFloat(_vertical, vertical);
    }
}

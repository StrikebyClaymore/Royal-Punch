using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BaseAnimation : MonoBehaviour
{
    protected Animator animator;
    private float _animDuration;

    private readonly int _idle = Animator.StringToHash("Idle");
    private readonly int _punch = Animator.StringToHash("Punch");
    private readonly int _idleNoExit = Animator.StringToHash("IdleNoExit");
    private readonly int _finishPunch = Animator.StringToHash("FinishPunch");
    
    public Action OnAnimationCompleted;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartPunch() => animator.SetBool(_punch, true);

    public void StopPunch() => animator.SetBool(_punch, false);

    public void FinishPunch() => animator.Play(_finishPunch);

    public void StartIdle(bool noExitTime = false)
    {
        animator.SetBool(_idle, true);
        if (noExitTime)
        {
            animator.SetTrigger(_idleNoExit);
        }
    }

    public void StopIdle() => animator.SetBool(_idle, false);
    
    public void On() => animator.enabled = true;

    public void Off() => animator.enabled = false;

    public void Toggle(bool enable) => animator.enabled = enable;

    public void SetSpeed(float speed) => animator.speed = speed;

    public void DisableOtherAnimations(string anim)
    {   
        foreach (var parameter in animator.parameters)
            if (parameter.name != anim)
                animator.SetBool(parameter.name, false);
    }

    public void DisableAllAnimations()
    {   
        foreach (var parameter in animator.parameters)
            animator.SetBool(parameter.name, false);
    }

    public void AddAnimationCompletedEvent(int layerIdx = 0, float percentPassed = 0)
    {
        _animDuration = GetAnimationTime(layerIdx);
        var secondsPassed = (_animDuration / 100) * percentPassed;
        StartCoroutine(AnimationCompleted(secondsPassed));
    }

    protected virtual IEnumerator AnimationCompleted(float secondsPassed = 0)
    {
        yield return new WaitForSeconds(_animDuration - secondsPassed);
        OnAnimationCompleted?.Invoke();
    }

    public float GetAnimationTime(int layerIdx) => animator.GetCurrentAnimatorStateInfo(layerIdx).length;
}

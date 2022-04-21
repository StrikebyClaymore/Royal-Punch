using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BodyAnimator : MonoBehaviour
{
    protected Animator _animator;
    protected const string IdleAnimation = "Idle";
    protected const string PunchAnimation = "Punch";

    public Action OnAttackCooldown;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public virtual void SetIdle()
    {
        _animator.SetBool(IdleAnimation, true);
    }
    
    public void SetPunch()
    {
        _animator.SetBool(IdleAnimation, false);
        _animator.SetBool(PunchAnimation, true);
        StartCoroutine(AttackCooldown());
    }
    
    protected virtual IEnumerator AttackCooldown()
    {
        var animDuration = _animator.GetCurrentAnimatorStateInfo(1).length;
        
        yield return new WaitForSeconds(animDuration);

        OnAttackCooldown?.Invoke();

        if (_animator.GetCurrentAnimatorStateInfo(1).IsName(PunchAnimation))
            StartCoroutine(AttackCooldown());
    }
    
    public void StopPunch()
    {
        _animator.SetBool(PunchAnimation, false);   
    }
    
    public void On() => _animator.enabled = true;
    
    public void Off() => _animator.enabled = false;

    public void SetSpeed(float speed) => _animator.speed = speed;
}
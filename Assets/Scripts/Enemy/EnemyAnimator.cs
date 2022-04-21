using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : BodyAnimator
{
    private const string SuperAttack = "SuperAttack";
    private float _superAttackAnimDuration;
    public Action OnSuperAttackCompleted;
    
    public override void SetIdle()
    {
        _animator.SetBool(PunchAnimation, false);
        base.SetIdle();
    }

    public void SetSuperAttack()
    {
        _superAttackAnimDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
        _animator.SetTrigger(SuperAttack);
    }

    public void ContinueSuperAttack(float chargingTime)
    {
        StartCoroutine(SuperAttackCompleted(chargingTime));
    }
    
    protected virtual IEnumerator SuperAttackCompleted(float chargingTime)
    {
        yield return new WaitForSeconds(_superAttackAnimDuration - chargingTime);
        OnSuperAttackCompleted?.Invoke();
    }
}

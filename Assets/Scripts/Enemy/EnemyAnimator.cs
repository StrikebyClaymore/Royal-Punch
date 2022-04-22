using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : BodyAnimator
{
    private const string SuperAttack = "SuperAttack";
    public Action OnSuperAttackCompleted;
    
    public override void SetIdle()
    {
        _animator.SetBool(PunchAnimation, false);
        base.SetIdle();
    }

    public void SetSuperAttack()
    {
        _animator.SetTrigger(SuperAttack);
    }

    public void ContinueSuperAttack(float chargingTime)
    {
        StartCoroutine(SuperAttackCompleted(chargingTime));
    }
    
    protected virtual IEnumerator SuperAttackCompleted(float chargingTime)
    {
        yield return new WaitForEndOfFrame();
        var superAttackAnimDuration = _animator.GetCurrentAnimatorStateInfo(1).length;
        yield return new WaitForSeconds(superAttackAnimDuration - chargingTime);
        OnSuperAttackCompleted?.Invoke();
    }
}

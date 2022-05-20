using System;
using Extensions;
using UnityEngine;

[RequireComponent(typeof(Boxer))]
public abstract class BaseAttack : MonoBehaviour
{
    protected Boxer boxer;
    protected internal AttackRangeDetector attackRangeDetector;
    [SerializeField] private Hand _leftHand;
    [SerializeField] private Hand _rightHand;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _knockOutForce = 25000f;
    protected bool finishPunch = false;
    
    protected virtual void Awake()
    {
        boxer = GetComponent<Boxer>();
        gameObject.TryGetComponentInChildren(true, out attackRangeDetector);
    }

    public void LeftPunch()
    {
        Punch(_leftHand);
    }
        
    public void RightPunch()
    {
        Punch(_rightHand);
    }

    protected virtual void Punch(Hand hand)
    {
        if(hand.Body is null)
            return;
        
        if(finishPunch)
            hand.Body.KnockOut(_knockOutForce, _damage);
        else
            hand.Body.GetHit(transform.position, _damage);
        
        if (hand.Body.IsLastHit(_damage))
        {
            finishPunch = true;
            boxer.animationSystem.StopPunch();
            boxer.animationSystem.FinishPunch();
        }
    }
        
    public void FinishPunch()
    {
        Punch(_rightHand);
    }

    protected virtual void StartBattle() { }

    protected internal virtual void SetDamage(int value) => _damage = value;

    public void TargetEnterRange()
    {
        Debug.Log("ENTER IN " + name);
        //boxer.animationSystem.StopIdle();
        /*if(_rightHand.Body == null && _leftHand.Body == null)
            return;*/
        if(finishPunch == false)
            boxer.animationSystem.StartPunch();
        else
            boxer.animationSystem.FinishPunch();
    }

    private void TargetExitRange()
    {
        Debug.Log("EXIT IN " + name);
        boxer.animationSystem.StopPunch();
        boxer.animationSystem.StartIdle();
    }

    protected virtual void ConnectActions()
    {
        attackRangeDetector.OnTargetEnterRange += TargetEnterRange;
        attackRangeDetector.OnTargetExitRange += TargetExitRange;
        GameManager.Camera.OnBattleStarting += StartBattle;
    }
}
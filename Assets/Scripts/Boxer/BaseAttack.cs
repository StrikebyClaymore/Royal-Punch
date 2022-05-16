using System;
using Extensions;
using UnityEngine;

[RequireComponent(typeof(Boxer))]
public abstract class BaseAttack : MonoBehaviour
{
    private Boxer _boxer;
    private AttackRangeDetector _attackRangeDetector;
    [SerializeField] private Hand _leftHand;
    [SerializeField] private Hand _rightHand;
    [SerializeField] private int _damage = 10;
    protected bool finishPunch = false;
    
    private void Awake()
    {
        _boxer = GetComponent<Boxer>();
        gameObject.TryGetComponentInChildren(true, out _attackRangeDetector);
    }

    private void Start()
    {
        ConnectActions();
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
        hand.Body.GetHit(transform.position, _damage);
        if (hand.Body.IsLastHit(_damage))
        {
            finishPunch = true;
        }
    }
        
    public void FinishPunch()
    {
        Punch(_rightHand);
    }
    
    private void TargetEnterRange()
    {
        _boxer.animationSystem.StopIdle();
        if(finishPunch == false)
            _boxer.animationSystem.StartPunch();
        else
            _boxer.animationSystem.FinishPunch();
    }

    private void TargetExitRange()
    {
        _boxer.animationSystem.StopPunch();
        _boxer.animationSystem.StartIdle();
    }

    private void ConnectActions()
    {
        _attackRangeDetector.OnTargetEnterRange += TargetEnterRange;
        _attackRangeDetector.OnTargetExitRange += TargetExitRange;
    }
}
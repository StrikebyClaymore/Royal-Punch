using System.Collections;
using Extensions;
using UnityEngine;

[RequireComponent(typeof(Boxer))]
public abstract class BaseAttack : MonoBehaviour
{
    protected Boxer boxer;
    protected internal AttackRangeDetector attackRangeDetector;
    [SerializeField] private Hand _leftHand;
    [SerializeField] private Hand _rightHand;
    [SerializeField] private protected int damage = 10;
    [SerializeField] private protected float knockOutForce = 25000f;
    protected bool finishPunch = false;
    [SerializeField] protected int attackLayer;
    
    protected virtual void Awake()
    {
        boxer = GetComponent<Boxer>();
        gameObject.TryGetComponentInChildren(true, out attackRangeDetector);
    }

    public void LeftPunchEvent()
    {
        Punch(_leftHand);
    }
        
    public void RightPunchEvent()
    {
        Punch(_rightHand);
    }

    protected virtual void Punch(Hand hand)
    {
        if (hand.Body == null)
            return;

        if(finishPunch)
            hand.Body.KnockOut(knockOutForce, damage);
        else
            hand.Body.GetHit(transform.position, damage);

        if (!hand.Body.IsLastHit(damage))
            return;
        
        finishPunch = true;
        StartCoroutine(CheckLock());
    }

    protected IEnumerator CheckLock()
    {
        yield return new WaitForEndOfFrame();
        boxer.Lock();
        yield return new WaitForSeconds(0.1f);
        boxer.animationSystem.FinishPunch();
    }
    
    public void FinishPunchEvent()
    {
        Punch(_rightHand);
    }

    protected virtual void StartBattle() { }

    protected internal virtual void SetDamage(int value) => damage = value;

    private void TargetEnterRange()
    {
        if (finishPunch == false)
            boxer.animationSystem.StartPunch();
        else
            boxer.animationSystem.FinishPunch();
    }

    private void TargetExitRange()
    {
        boxer.animationSystem.StopPunch();
    }

    protected virtual void ConnectActions()
    {
        attackRangeDetector.OnTargetEnterRange += TargetEnterRange;
        attackRangeDetector.OnTargetExitRange += TargetExitRange;
        GameManager.Camera.OnBattleStarting += StartBattle;
    }
}
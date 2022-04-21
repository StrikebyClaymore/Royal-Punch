using System;
using UnityEngine;

[RequireComponent(typeof(BodyAnimator),
    typeof(RagdollStateChanger))]
public abstract class BaseAttack : MonoBehaviour
{
    protected BodyAnimator animator;
    private RagdollStateChanger _ragdollStateChanger;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float forcePower = 10000f;
    private bool _leftHandCooldown = true;
    private bool _rightHandCooldown = false;
    
    public enum HandType
    {
        Left = 0,
        Right = 1,
    }

    protected virtual void Awake()
    {
        animator = GetComponent<BodyAnimator>();
        _ragdollStateChanger = GetComponent<RagdollStateChanger>();
        ConnectActions();
    }

    public void StartAttack()
    {
        animator.SetPunch();
    }

    public void StopAttack()
    {
        animator.StopPunch();
    }
    
    public virtual void Attack(HitArea hitArea, HandType type)
    {
        if(AttackInCooldown(type))
            return;
        if (type == HandType.Left)
            _leftHandCooldown = false;
        else
            _rightHandCooldown = false;
        hitArea.Hit(damage, transform.forward * forcePower);
    }

    private bool AttackInCooldown(HandType type)
    {
        if (type == HandType.Left)
            return _leftHandCooldown == false;
        return _rightHandCooldown == false;
    }
    
    private void AttackCooldown()
    {
        _leftHandCooldown = true;
        _rightHandCooldown = true;
    }
    
    protected virtual void ConnectActions()
    {
        animator.OnAttackCooldown += AttackCooldown;
    }
}
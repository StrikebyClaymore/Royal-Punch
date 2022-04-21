using System;
using UnityEngine;

[RequireComponent(typeof(RagdollStateChanger),
    typeof(BaseAttack),
    typeof(BodyAnimator))]
public abstract class Body : MonoBehaviour
{
    protected RagdollStateChanger ragdollStateChanger;
    private BaseAttack _attack;
    protected BodyAnimator animator;
    [SerializeField] protected Health health;
    [SerializeField] private Collider _bodyCollider;
    [SerializeField] private Collider _detectBodyCollider;
    [SerializeField] private Collider _hitAreaCollider;
    public HitArea hitArea;
    
    protected virtual void Awake()
    {
        ragdollStateChanger = GetComponent<RagdollStateChanger>();
        _attack = GetComponent<BaseAttack>();
        animator = GetComponent<BodyAnimator>();
        ConnectActions();
    }

    public virtual void Win()
    {
        DisableColliders();
        _attack.StopAttack();
    }

    protected virtual void Die()
    {
        DisableColliders();
        ragdollStateChanger.On();
    }

    private void DisableColliders()
    {
        _bodyCollider.enabled = false;
        _detectBodyCollider.enabled = false;
        _hitAreaCollider.enabled = false;
    }
    
    protected virtual void ConnectActions()
    {
        health.OnDie += Die;
    }
}
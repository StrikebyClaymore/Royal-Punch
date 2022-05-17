using System;
using UnityEngine;
using Extensions;

public abstract class Boxer : MonoBehaviour, IHitable
{
    protected internal RagdollSystem ragdollSystem;
    protected internal BaseAnimation animationSystem;
    private Health _health;

    protected virtual void Awake()
    {
        ragdollSystem = GetComponent<RagdollSystem>();
        animationSystem = GetComponent<BaseAnimation>();
        gameObject.TryGetComponentInChildren(true, out _health);
    }

    public void GetHit(Vector3 hitPoint, int damage)
    {
        _health.ApplyDamage(damage);
    }

    public void KnockOut(float force, int damage)
    {
        //ragdollSystem.StartFall(force, true);
        //_health.ApplyDamage(damage);
    }

    public bool IsLastHit(int damage) => _health.IsLastHp(damage);
    
    protected virtual void Die()
    {
        _health.Toggle(false);
    }

    protected virtual void ConnectActions()
    {
        _health.OnDie += Die;
    }
}

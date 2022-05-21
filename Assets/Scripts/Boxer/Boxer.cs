using UnityEngine;
using Extensions;

public abstract class Boxer : MonoBehaviour, IHitable
{
    protected internal RagdollSystem ragdollSystem;
    protected internal BaseAnimation animationSystem;
    protected Health health;
    [SerializeField] protected BoxerConfig config;

    protected virtual void Awake()
    {
        ragdollSystem = GetComponent<RagdollSystem>();
        animationSystem = GetComponent<BaseAnimation>();
        gameObject.TryGetComponentInChildren(true, out health);
    }

    public void GetHit(Vector3 hitPoint, int damage)
    {
        health.ApplyDamage(damage);
    }

    public void KnockOut(float force, int damage)
    {
        ragdollSystem.KnockOut(force, health.IsLastHp(damage) == false);
        health.ApplyDamage(damage);
    }

    public bool IsLastHit(int damage) => health.IsLastHp(damage);

    protected virtual void ApplyUpgrades() { }
    
    protected virtual void Die()
    {
        health.Toggle(false);
    }

    protected virtual void ConnectActions()
    {
        health.OnDie += Die;
    }
}

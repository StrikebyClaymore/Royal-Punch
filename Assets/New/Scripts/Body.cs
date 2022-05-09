using System;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(RagdollSystem),
        typeof(AnimationBase))]
    public abstract class Body : MonoBehaviour, IHitable
    {
        protected RagdollSystem ragdollSystem;
        protected AnimationBase animationSysem;
        [SerializeField] protected Health health;

        protected virtual void Awake()
        {
            ragdollSystem = GetComponent<RagdollSystem>();
            animationSysem = GetComponent<AnimationBase>();
        }
        
        public virtual void GetHit(Vector3 hitPoint, int damage)
        {
            Debug.Log(name + " GetHit");
            health.ApplyDamage(damage);
        }
        
        public virtual void KnockOut(float force)
        {
            Debug.Log(name + " KnockOut");
            ragdollSystem.StartFall(force);
        }

        protected virtual void Die(){}

        protected virtual void ConnectActions()
        {
            health.OnDie += Die;
        }
    }
}
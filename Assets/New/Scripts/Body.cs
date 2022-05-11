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
        [SerializeField] protected AttackRangeDetecter attackRangeDetecter;

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
            ragdollSystem.StartFall(force, true);
        }

        protected virtual void Die()
        {
            health.Toggle(false);
        }

        private void TargetEnterRange()
        {
            animationSysem.StartPunch();
        }

        private void TargetExitRange()
        {
            animationSysem.StopPunch();
        }
        
        protected virtual void ConnectActions()
        {
            health.OnDie += Die;
            attackRangeDetecter.OnTargetEnterRange += TargetEnterRange;
            attackRangeDetecter.OnTargetExitRange += TargetExitRange;
        }
    }
}
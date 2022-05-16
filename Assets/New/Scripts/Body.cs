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
        protected bool finishPunch; 

        protected virtual void Awake()
        {
            ragdollSystem = GetComponent<RagdollSystem>();
            animationSysem = GetComponent<AnimationBase>();
        }
        
        public virtual void GetHit(Vector3 hitPoint, int damage)
        {
            health.ApplyDamage(damage);
        }
        
        public virtual void KnockOut(float force)
        {
            Debug.Log(name + " KnockOut");
            ragdollSystem.StartFall(force, true);
        }

        public bool IsLastHit(int damage) => health.IsLastHp(damage);
        
        public void StartPunch()
        {
            if (finishPunch)
            {
                animationSysem.FinishPunch();
            }
        }

        protected virtual void Die()
        {
            health.Toggle(false);
        }

        private void TargetEnterRange()
        {
            animationSysem.StopIdle();
            if(finishPunch == false)
                animationSysem.StartPunch();
            else
                animationSysem.FinishPunch();
        }

        private void TargetExitRange()
        {
            Debug.Log("EXIT");
            animationSysem.StopPunch();
            animationSysem.StartIdle();
        }
        
        protected virtual void ConnectActions()
        {
            health.OnDie += Die;
            attackRangeDetecter.OnTargetEnterRange += TargetEnterRange;
            attackRangeDetecter.OnTargetExitRange += TargetExitRange;
        }
    }
}
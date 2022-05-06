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

        protected virtual void Awake()
        {
            ragdollSystem = GetComponent<RagdollSystem>();
            animationSysem = GetComponent<AnimationBase>();
        }

        public virtual void GetHit(Vector3 hitPoint)
        {
            Debug.Log(name + " GetHit");
        }
        
        public virtual void KnockOut(float force)
        {
            Debug.Log(name + " KnockOut");
            ragdollSystem.StartFall(force);
        }
    }
}
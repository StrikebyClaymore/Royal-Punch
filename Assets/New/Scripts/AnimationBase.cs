using System;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(Animator))]
    public class AnimationBase : MonoBehaviour
    {
        protected Animator animator;
        private readonly int _horizontal = Animator.StringToHash("Horizontal");
        private readonly int _vertical = Animator.StringToHash("Vertical");
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _punch = Animator.StringToHash("Punch");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void StartPunch() => animator.SetBool(_punch, true);

        public void StopPunch() => animator.SetBool(_punch, false);
        
        public void StartMove() => animator.SetBool(_move, true);

        public void StopMove() => animator.SetBool(_move, false);

        public void SetDirection(float horizontal, float vertical)
        {
            animator.SetFloat(_horizontal, horizontal);
            animator.SetFloat(_vertical, vertical);
        }

        public void On() => animator.enabled = true;
        
        public void Off() => animator.enabled = false;
        
        public void Toggle(bool enable) => animator.enabled = enable;
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(Animator))]
    public class AnimationBase : MonoBehaviour
    {
        protected Animator animator;
        private readonly int _horizontal = Animator.StringToHash("Horizontal");
        private readonly int _vertical = Animator.StringToHash("Vertical");
        private readonly int _idle = Animator.StringToHash("Idle");
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _punch = Animator.StringToHash("Punch");
        
        private float _animDuration;
        
        public Action OnAnimationCompleted;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void StartPunch() => animator.SetBool(_punch, true);

        public void StopPunch() => animator.SetBool(_punch, false);
        
        public void StartMove() => animator.SetBool(_move, true);

        public void StopMove() => animator.SetBool(_move, false);
        
        public void StartIdle() => animator.SetBool(_idle, true);

        public void SetDirection(float horizontal, float vertical)
        {
            animator.SetFloat(_horizontal, horizontal);
            animator.SetFloat(_vertical, vertical);
        }

        public void On() => animator.enabled = true;
        
        public void Off() => animator.enabled = false;
        
        public void Toggle(bool enable) => animator.enabled = enable;

        public void SetSpeed(float speed) => animator.speed = speed;

        public void AddAnimationCompletedEvent(int layerIdx, float percentPassed = 0)
        {
            _animDuration = GetAnimationTime(layerIdx);
            var secondsPassed = (_animDuration / 100) * percentPassed;
            StartCoroutine(AnimationCompleted(secondsPassed));
        }
    
        protected virtual IEnumerator AnimationCompleted(float secondsPassed = 0)
        {
            yield return new WaitForSeconds(_animDuration - secondsPassed);
            OnAnimationCompleted?.Invoke();
        }
        
        private float GetAnimationTime(int layerIdx) => animator.GetCurrentAnimatorStateInfo(layerIdx).length;
    }
}
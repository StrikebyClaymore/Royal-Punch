using System;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(Animator))]
    public class AnimationBase : MonoBehaviour
    {
        private Animator _animator;
        private readonly int _horizontal = Animator.StringToHash("Horizontal");
        private readonly int _vertical = Animator.StringToHash("Vertical");
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _punch = Animator.StringToHash("Punch");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void StartPunch() => _animator.SetBool(_punch, true);

        public void StopPunch() => _animator.SetBool(_punch, false);
        
        public void StartMove() => _animator.SetBool(_move, true);

        public void StopMove() => _animator.SetBool(_move, false);

        public void SetDirection(float horizontal, float vertical)
        {
            _animator.SetFloat(_horizontal, horizontal);
            _animator.SetFloat(_vertical, vertical);
        }
    }
}
using System;
using UnityEngine;

namespace New
{
    public class AnimationRigging : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Vector3 _targetBasePosition;
        private const float TargetOffset = 1f; 
        private const float ReturnSpeed = 5f;

        private void Awake()
        {
            _targetBasePosition = _target.position;
        }

        public void AddHitReaction(Vector3 direction)
        {
            direction.y = 0;
            _target.Translate(direction * TargetOffset);
        }

        private void FixedUpdate()
        {
            if(Vector3.Distance(_target.position, _targetBasePosition) < ReturnSpeed * Time.fixedDeltaTime)
                return;
            var direction = (_target.position - _targetBasePosition).normalized;
            direction.y = 0;
            var transition = ReturnSpeed * Time.fixedDeltaTime * direction;
            _target.Translate(transition);
        }
    }
}

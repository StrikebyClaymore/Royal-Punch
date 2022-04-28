using System;
using UnityEngine;

namespace New
{
    public class AnimationRigging : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Vector3 _targetBasePosition;
        [SerializeField] private float TargetOffset = 1f; 
        [SerializeField] private float ReturnSpeed = 5f;

        private Vector3 _hitDirection;
        
        private void Awake()
        {
            _targetBasePosition = _target.position;
        }

        public void AddHitReaction(Vector3 hitPoint)
        {
            var direction = (transform.position - hitPoint).normalized;
            direction.y = 0;
            _target.Translate(direction * TargetOffset, Space.World);
            _hitDirection = direction;
        }

        private void FixedUpdate()
        {
            if(Vector3.Distance(_target.position, _targetBasePosition) < ReturnSpeed * Time.fixedDeltaTime)
                return;
            var direction = (_target.position - _targetBasePosition).normalized;
            direction.y = 0;
            var transition = ReturnSpeed * Time.fixedDeltaTime * direction;
            _target.Translate(-transition, Space.World);
        }
        
        private void OnDrawGizmos()
        {
            var position = _target.position;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, position + _hitDirection * 10f);
            /*var position = _target.position;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, _targetBasePosition);
            var direction = (_targetBasePosition - position).normalized;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, position + direction * 10f);*/
            
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace New
{
    [CreateAssetMenu(fileName = "SuperAttackConfig", order = 51)]
    public class SuperAttackConfig : ScriptableObject
    {
        public int id;
        [SerializeField] private Material _material;
        [Range(0, 1)]
        [SerializeField] private float _startNearPlane;
        [Range(0, 1)]
        [SerializeField] private float _endNearPlane;
        [Range(0, 1)]
        [SerializeField] private float _startFarPlane;
        [Range(0, 1)]
        [SerializeField] private float _endFarPlane;
        
        [SerializeField] private Vector3 _startColliderCenter;
        [SerializeField] private Vector3 _startColliderSize;
        [SerializeField] private Vector3 _endColliderCenter;
        [SerializeField] private Vector3 _endColliderSize;

        [HideInInspector]
        public Collider collider;

        private static readonly int NearPlane = Shader.PropertyToID("_NearPlane");
        private static readonly int FarPlane = Shader.PropertyToID("_FarPlane");
        private float _nearPlane;
        private float _farPlane;
        
        [SerializeField] private float _speed = 1f;

        public Action OnAttack;

        public void Process(EnemyAttack.SuperStates state)
        {
            if (state == EnemyAttack.SuperStates.Charge)
            {
                _nearPlane = Mathf.Min(_endNearPlane, _nearPlane + Time.deltaTime);
                _farPlane = Mathf.Min(_endFarPlane, _farPlane + Time.deltaTime);
            }
            else if (state == EnemyAttack.SuperStates.Attack)
            {
                _nearPlane = Mathf.Min(1, _nearPlane + _speed * Time.deltaTime);

                ColliderProcess();
                
                if(_nearPlane == 1)
                    OnAttack?.Invoke();
            }
            _material.SetFloat(NearPlane, _nearPlane);
            _material.SetFloat(FarPlane, _farPlane);
        }

        public void ResetParams()
        {
            _nearPlane = _startNearPlane;
            _farPlane = _startFarPlane;
            _material.SetFloat(NearPlane, _startNearPlane);
            _material.SetFloat(FarPlane, _startFarPlane);
            ResetCollider();
        }

        private void ColliderProcess()
        {
            Vector3 center;
            switch (id)
            {
                case 1:
                case 2:
                    var sColl = ((SphereCollider) collider);
                    center = new Vector3(_endColliderCenter.x, _endColliderCenter.y, _endColliderCenter.z * _nearPlane);
                    var radius = _endColliderSize.x * _nearPlane;
                    sColl.center = center;
                    sColl.radius = radius;
                    break;
                case 3:
                    var bColl = ((BoxCollider) collider);
                    center = new Vector3(_endColliderCenter.x, _endColliderCenter.y, _endColliderCenter.z * _nearPlane);
                    var size = new Vector3(_endColliderSize.x * _nearPlane, _endColliderSize.y, _endColliderSize.z);
                    bColl.center = center;
                    bColl.size = size;
                    break;
            }
        }
        
        private void ResetCollider()
        {
            switch (id)
            {
                case 1:
                case 2:
                    var sColl = ((SphereCollider) collider);
                    sColl.center = _startColliderCenter;
                    sColl.radius = _startColliderSize.x;
                    break;
                case 3:
                    var bColl = ((BoxCollider) collider);
                    bColl.center = _startColliderCenter;
                    bColl.size = _startColliderSize;
                    break;
            }
        }
    }
}
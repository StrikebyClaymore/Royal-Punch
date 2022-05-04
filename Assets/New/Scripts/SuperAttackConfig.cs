using System;
using UnityEngine;

namespace New
{
    [CreateAssetMenu(fileName = "SuperAttackConfig", order = 51)]
    public class SuperAttackConfig : ScriptableObject
    {
        public int id;
        [SerializeField] private Material _material;
        [Range(0, 1)]
        public float startNearPlane;
        [Range(0, 1)]
        public float endNearPlane;
        [Range(0, 1)]
        public float startFarPlane;
        [Range(0, 1)]
        public float endFarPlane;

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
                _nearPlane = Mathf.Min(endNearPlane, _nearPlane + Time.deltaTime);
                _farPlane = Mathf.Min(endFarPlane, _farPlane + Time.deltaTime);
            }
            else if (state == EnemyAttack.SuperStates.Attack)
            {
                _nearPlane = Mathf.Min(1, _nearPlane + _speed * Time.deltaTime);
                if(_nearPlane == 1)
                    OnAttack?.Invoke();
            }
            _material.SetFloat(NearPlane, _nearPlane);
            _material.SetFloat(FarPlane, _farPlane);
        }

        public void ResetParams()
        {
            _nearPlane = startNearPlane;
            _farPlane = startFarPlane;
            _material.SetFloat(NearPlane, startNearPlane);
            _material.SetFloat(FarPlane, startFarPlane);
        }
        
    }
}
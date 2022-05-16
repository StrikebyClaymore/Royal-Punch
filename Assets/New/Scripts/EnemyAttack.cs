using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    /*[RequireComponent(typeof(AnimationEnemy))]*/
    public class EnemyAttack : MonoBehaviour
    {
        private AnimationEnemy _animation;
        private Transform _player;
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private AttackEffects _effects;
        [SerializeField] private AttackColliders _colliders;
        private const int SuperAttackLayer = 0;
        private Timer _superAttackTimer;
        [SerializeField] private int _SuperAttackChargeTime;
        private float _animationPercentPassed;
        private SuperAttackConfig _currentSuperAttack;
        [SerializeField] private float _rotationSpeed = 1f;

        public enum SuperStates
        {
            None = 0,
            Start = 1,
            Charge = 2,
            Continue = 3,
            Attack = 4,
            AttackEnd = 5,
            Tried = 6
        }
        private SuperStates _superState = SuperStates.None;

        /*public enum SuperTypes
        {
            Circle,
            Conus
        }*/
        
        private void Awake()
        {
            _animation = GetComponent<AnimationEnemy>();
            
            _superAttackTimer = gameObject.AddComponent<Timer>();
            _superAttackTimer.Init(transform, _SuperAttackChargeTime, SuperAttackTimerTimeOut);
        }

        private void Start()
        {
            _player = GameManager.Player2.transform;
        }

        private void Update()
        {
            if (_superState == SuperStates.Charge || _superState == SuperStates.Attack)
            {
                _currentSuperAttack.Process(_superState);
            }
        }

        private void FixedUpdate()
        {
            if(_superState != SuperStates.None)
                return;
            Rotate();
        }
        
        public void StartSuperAttackPressed(int attackNumber)
        {
            ChangeSuperState(SuperStates.Start);
            _currentSuperAttack = _config.GetSuper(attackNumber);
            _currentSuperAttack.OnAttack += SuperAttack;
            _animation.StartSuper(_currentSuperAttack.id);
        }
        
        public void StartChargeSuper(float animationSecondsPassed)
        {
            ChangeSuperState(SuperStates.Charge);
            _animationPercentPassed = animationSecondsPassed;
            _animation.SetSpeed(0);
            _animation.OnAnimationCompleted += SuperAttackEnd;
            _superAttackTimer.Enable();
            
            _colliders.SetCollider(_currentSuperAttack);
            _effects.AreaSetVisible(_currentSuperAttack.id, true);
        }

        private void SuperAttackTimerTimeOut()
        {
            StartCoroutine(SuperAttackContinue());
            _animation.SetSpeed(1);
        }
        
        private IEnumerator SuperAttackContinue()
        {
            ChangeSuperState(SuperStates.Continue);
            yield return new WaitForEndOfFrame();
            _animation.AddAnimationCompletedEvent(SuperAttackLayer, _animationPercentPassed);
            _superAttackTimer.ResetTime();
        }

        private void ApplySuperAttack()
        {
            ChangeSuperState(SuperStates.Attack);
            _colliders.SetEnable(_currentSuperAttack.id, true);
            _effects.Play(_currentSuperAttack.id);
        }
        
        private void SuperAttack()
        {
            ChangeSuperState(SuperStates.AttackEnd);
        }
        
        private void SuperAttackEnd()
        {
            ChangeSuperState(SuperStates.Tried);
            _colliders.SetEnable(_currentSuperAttack.id, false);
            _effects.AreaSetVisible(_currentSuperAttack.id, false);
            _animationPercentPassed = 0;
            _currentSuperAttack.ResetParams();
            _animation.OnAnimationCompleted -= SuperAttackEnd;
            _currentSuperAttack.OnAttack -= SuperAttack;
            StartCoroutine(TriedContinue());
        }       

        private IEnumerator TriedContinue()
        {
            yield return new WaitForEndOfFrame();
            _animation.OnAnimationCompleted += TriedEnd;
            _animation.AddAnimationCompletedEvent(SuperAttackLayer);
        }
        
        private void TriedEnd()
        {
            ChangeSuperState(SuperStates.None);
            _animation.OnAnimationCompleted -= TriedEnd;
        }
        
        private void ChangeSuperState(SuperStates newState) => _superState = newState;

        private void Rotate()
        {
            var relativePos = _player.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            var rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = rotation;
        }
        
    }
}
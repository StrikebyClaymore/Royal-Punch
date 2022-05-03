using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(AnimationEnemy))]
    public class EnemyAttack : MonoBehaviour
    {
        private AnimationEnemy _animation;
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private GameObject _circleEffect;
        [SerializeField] private GameObject _conusEffect;
        private const int SuperAttackLayer = 0;
        private Timer _superAttackTimer;
        [SerializeField] private int _SuperAttackChargeTime;
        private float _animationSecondsPassed;
        private SuperAttackConfig _currentSuperAttack;

        public enum SuperStates
        {
            None = 0,
            Start = 1,
            Charge = 2,
            Continue = 3,
            Tried = 4
        }
        private SuperStates _superState = SuperStates.None;

        private void Awake()
        {
            _animation = GetComponent<AnimationEnemy>();
            
            _superAttackTimer = gameObject.AddComponent<Timer>();
            _superAttackTimer.Init(transform, _SuperAttackChargeTime, SuperAttackContinue);
        }

        private void Start()
        {
            ConnectActions();
        }

        private void Update()
        {
            if(_superState == SuperStates.Charge || _superState == SuperStates.Continue)
                _currentSuperAttack.Process(_superState);
            else if (_superState == SuperStates.Start)
                _animationSecondsPassed += Time.deltaTime;
        }

        public void StartSuperAttack(int attackNumber)
        {
            ChangeSuperState(SuperStates.Start);
            _currentSuperAttack = _config.GetSuper(attackNumber);
            _animation.StartSuper(_currentSuperAttack.id);
        }
        
        public void StartChargeSuper(float animationSecondsPassed)
        {
            ChangeSuperState(SuperStates.Charge);
            _animationSecondsPassed = animationSecondsPassed;
            _animation.SetSpeed(0);
            _superAttackTimer.Enable();
            
            if(_currentSuperAttack.id == 1 || _currentSuperAttack.id == 2)
                _circleEffect.SetActive(true);
            else if (_currentSuperAttack.id == 3)
                _conusEffect.SetActive(true);
        }
        
        private void SuperAttackContinue()
        {
            ChangeSuperState(SuperStates.Continue);
            _animation.SetSpeed(1);
            _animation.AddAnimationCompletedEvent(SuperAttackLayer, _animationSecondsPassed);
            _superAttackTimer.ResetTime();
        }

        private void SuperAttackEnd()
        {
            ChangeSuperState(SuperStates.Tried);
            _animationSecondsPassed = 0;
            _currentSuperAttack.ResetParams();
        }
        
        private void ChangeSuperState(SuperStates newState) => _superState = newState;

        private void ConnectActions()
        {
            _animation.OnAnimationCompleted += SuperAttackEnd;
        }
    }
}
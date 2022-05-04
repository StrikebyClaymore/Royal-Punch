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
            Attack = 4,
            AttackEnd = 5,
            Tried = 6
        }
        private SuperStates _superState = SuperStates.None;

        private void Awake()
        {
            _animation = GetComponent<AnimationEnemy>();
            
            _superAttackTimer = gameObject.AddComponent<Timer>();
            _superAttackTimer.Init(transform, _SuperAttackChargeTime, SuperAttackTimerTimeOut);
        }

        private void Start()
        {
            ConnectActions();
        }

        private void Update()
        {
            if(_superState == SuperStates.Charge || _superState == SuperStates.Attack)
                _currentSuperAttack.Process(_superState);
            else if (_superState == SuperStates.Start)
                _animationSecondsPassed += Time.deltaTime;
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
            _animationSecondsPassed = animationSecondsPassed;
            _animation.SetSpeed(0);
            _superAttackTimer.Enable();

            SuperSetVisible(true);
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
            _animation.AddAnimationCompletedEvent(SuperAttackLayer, _animationSecondsPassed);
            _superAttackTimer.ResetTime();
        }

        private void ApplySuperAttack()
        {
            ChangeSuperState(SuperStates.Attack);
        }
        
        private void SuperAttack()
        {
            ChangeSuperState(SuperStates.AttackEnd);
        }
        
        private void SuperAttackEnd()
        {
            ChangeSuperState(SuperStates.Tried);
            SuperSetVisible(false);
            _animationSecondsPassed = 0;
            _currentSuperAttack.ResetParams();
            _currentSuperAttack.OnAttack -= SuperAttack;
        }
        
        private void ChangeSuperState(SuperStates newState) => _superState = newState;

        private void SuperSetVisible(bool visible)
        {
            if(_currentSuperAttack.id == 1 || _currentSuperAttack.id == 2)
                _circleEffect.SetActive(visible);
            else if (_currentSuperAttack.id == 3)
                _conusEffect.SetActive(visible);
        }
        
        private void ConnectActions()
        {
            _animation.OnAnimationCompleted += SuperAttackEnd;
        }
    }
}
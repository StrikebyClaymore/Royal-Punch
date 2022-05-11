using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(AnimationRigging))]
    public class Enemy : Body
    {
        private AnimationRigging _animationRigging;
        [SerializeField] private Hand _leftHand;
        [SerializeField] private Hand _rightHand;

        [SerializeField] private int _damage = 10;
        
        protected override void Awake()
        {
            base.Awake();
            GameManager.Enemy2 = this;
            _animationRigging = GetComponent<AnimationRigging>();
        }

        private void Start()
        {
            ConnectActions();
        }

        public void Win()
        {
            animationSysem.StopPunch();
            animationSysem.StartIdle(true);
        }
        
        public void LeftPunch()
        {
            if(_leftHand.Body is null)
                return;
            _leftHand.Body.GetHit(transform.position, _damage);
        }
        
        public void RightPunch()
        {
            if(_rightHand.Body is null)
                return;
            _rightHand.Body.GetHit(transform.position, _damage);
        }
        
        protected override void Die()
        {
            base.Die();
            ragdollSystem.EnemyStartFall(25000f);
            GameManager.PlayerController.LockInput(true);
        }

        public override void GetHit(Vector3 hitPoint, int damage)
        {
            base.GetHit(hitPoint, damage);
            _animationRigging.AddHitReaction(hitPoint);
        }
    }
}

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

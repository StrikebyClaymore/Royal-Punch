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
        private Transform _player;

        private void Awake()
        {
            GameManager.Enemy2 = this;
            _animationRigging = GetComponent<AnimationRigging>();
        }

        private void Start()
        {
            _player = GameManager.Player2.transform;
        }

        public override void GetHit(Vector3 hitPoint)
        {
            base.GetHit(hitPoint);
            var direction = (hitPoint - transform.position).normalized;
            _animationRigging.AddHitReaction(direction);
        }

        private void FixedUpdate()
        {
            transform.LookAt(_player);
        }
    }
}

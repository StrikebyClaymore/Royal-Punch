using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(AnimationRigging),
        typeof(AnimationEnemy))]
    public class Enemy : Body
    {
        private AnimationRigging _animationRigging;
        private AnimationEnemy _animation;

        private void Awake()
        {
            GameManager.Enemy2 = this;
            _animationRigging = GetComponent<AnimationRigging>();
            _animation = GetComponent<AnimationEnemy>();
        }

        public override void GetHit(Vector3 hitPoint)
        {
            base.GetHit(hitPoint);
            _animationRigging.AddHitReaction(hitPoint);
        }
    }
}

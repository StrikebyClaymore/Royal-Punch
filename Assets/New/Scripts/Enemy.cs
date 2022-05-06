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

        public override void GetHit(Vector3 hitPoint)
        {
            base.GetHit(hitPoint);
            _animationRigging.AddHitReaction(hitPoint);
        }
    }
}

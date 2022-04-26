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

        private void Awake()
        {
            _animationRigging = GetComponent<AnimationRigging>();
        }

        public override void GetHit(Vector3 hitPoint)
        {
            base.GetHit(hitPoint);
            var direction = (hitPoint - transform.position).normalized;
            _animationRigging.AddHitReaction(direction);
        }
    }
    
}

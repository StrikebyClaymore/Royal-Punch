using System;
using UnityEngine;

namespace New
{
    public class AnimationEnemy : AnimationBase
    {
        private const string Super = "Super_";

        public void StartSuper(int attackNumber)
        {
            animator.SetTrigger(Super+attackNumber);   
        }
    }
}
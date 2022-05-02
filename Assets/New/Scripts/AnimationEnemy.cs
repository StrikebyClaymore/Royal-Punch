using UnityEngine;

namespace New
{
    public class AnimationEnemy : AnimationBase
    {
        private const string Super = "Super_";
        
        public void StartSuper(int number) => animator.SetTrigger(Super+number);
        
        //public void 
    }
}
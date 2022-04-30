using System;
using UnityEngine;

namespace New
{
    public class Body : MonoBehaviour, IHitable
    {
        [HideInInspector]
        public BodyUpSystem upSystem;
        
        public virtual void GetHit(Vector3 hitPoint)
        {
            Debug.Log(name + " GetHit");
        }
    }
}
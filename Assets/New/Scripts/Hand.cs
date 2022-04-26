using System;
using UnityEngine;

namespace New
{
    public class Hand : MonoBehaviour
    {
        [HideInInspector]
        public IHitable Body;
        public enum HandTypes
        {
            Left,
            Right
        }
        public HandTypes type;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHitable>(out var body) == false)
                return;
            Body = body;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IHitable>(out var body) == false)
                return;
            if (body == Body)
                Body = null;
        }
    }
}
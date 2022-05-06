using System;
using UnityEngine;

namespace New
{
    public class Hand : MonoBehaviour
    {
        [HideInInspector]
        public IHitable Body;
        private Collider _collider;
        public enum HandTypes
        {
            Left,
            Right
        }
        public HandTypes type;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHitable>(out var body) == false || body == Body)
                return;
            Body = body;
            _collider = other;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IHitable>(out var body) == false)
                return;
            if (_collider == other)
            {
                Body = null;
                _collider = null;
            }
        }
    }
}
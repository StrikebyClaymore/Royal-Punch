using System;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(AnimationBase),
        typeof(CharacterController),
        typeof(RagdollSystem))]
    [RequireComponent(typeof(AnimationRigging),
        typeof(CharacterController))]
    public class BodyUpSystem : MonoBehaviour
    {
        private CharacterController _character;
        private AnimationBase _animation;
        private RagdollSystem _ragdollSystem;
        private AnimationRigging _animationRigging;

        [SerializeField] private Rigidbody _chest;
        private Vector3 _chestBasePosition;
        [SerializeField] private Transform _cameraTarget;


        private void Awake()
        {
            _animation = GetComponent<AnimationBase>();
            _character = GetComponent<CharacterController>();
            _ragdollSystem = GetComponent<RagdollSystem>();
            _animationRigging = GetComponent<AnimationRigging>();

            _chestBasePosition = _chest.position;
        }

        private void FixedUpdate()
        {
            var newPosition = _chest.transform.position;
            newPosition.y = 0;
            _cameraTarget.position = newPosition;
        }

        public void Hit()
        {
            enabled = true;
            _character.enabled = false;
            _ragdollSystem.On();
            GameManager.Camera2.SetFall(true);
            _chest.AddForce(-transform.forward * 25000f);
            
            Invoke(nameof(Up), 3f);
        }

        public void Up()
        {
            enabled = false;
            _ragdollSystem.Off();
            var newPosition = _chest.position - _chestBasePosition;
            newPosition.y = 0f;
            transform.Translate(newPosition);
            _character.enabled = true;
            GameManager.Camera2.SetFall(false);
        }
    }
}

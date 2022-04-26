using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

namespace New
{
    [RequireComponent(typeof(CharacterController),
        typeof(AnimationBase))]
    public class Player : Body
    {
        private CharacterController _character;
        private AnimationBase _animation;
        [SerializeField] private HitParticles _hitParticles;
        private Vector3 _direction;
        private const float MoveSpeed = 8f;
        [SerializeField] private Hand _leftHand;
        [SerializeField] private Hand _rightHand;
        
        private void Awake()
        {
            _character = GetComponent<CharacterController>();
            _animation = GetComponent<AnimationBase>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _animation.StartPunch();
            else if (Input.GetKeyUp(KeyCode.Space))
                _animation.StopPunch();

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
                _animation.StartMove();
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
                _animation.StopMove();
            
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(0,-3f,0);
            if (Input.GetKey(KeyCode.E))
                transform.Rotate(0,3f,0);

            _direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _animation.SetDirection(_direction.x, _direction.z);
        }

        private void FixedUpdate()
        {
            var motion = transform.rotation * Vector3.ClampMagnitude(MoveSpeed * _direction, MoveSpeed) * Time.fixedDeltaTime;
            _character.Move(motion);
        }

        public void LeftPunch()
        {
            if(_leftHand.Body is null)
                return;
            _hitParticles.StartHitParticles(_rightHand.type);
            _leftHand.Body.GetHit(transform.position);
        }
        
        public void RightPunch()
        {
            if(_rightHand.Body is null)
                return;
            _hitParticles.StartHitParticles(_leftHand.type);
            _rightHand.Body.GetHit(transform.position);
        }
    }
}
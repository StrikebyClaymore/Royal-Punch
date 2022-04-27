using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

namespace New
{
    [RequireComponent(typeof(AnimationBase))]
    public class Player : Body
    {
        private CharacterController _character;
        private AnimationBase _animation;
        [SerializeField] private HitParticles _hitParticles;
        private Transform _enemy;
        private GameCamera _camera;
        private Vector3 _direction;
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _minDist;
        [SerializeField] private Hand _leftHand;
        [SerializeField] private Hand _rightHand;
        
        private void Awake()
        {
            GameManager.Player2 = this;
            _animation = GetComponent<AnimationBase>();
            _character = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _enemy = GameManager.Enemy2.transform;
            _camera = GameManager.Camera2;
            ConnectActions();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _animation.StartPunch();
            else if (Input.GetKeyUp(KeyCode.Space))
                _animation.StopPunch();

            /*if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
                _animation.StartMove();
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
                _animation.StopMove();*/
            
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(0,-_rotationSpeed*Time.fixedDeltaTime,0);
            if (Input.GetKey(KeyCode.E))
                transform.Rotate(0,_rotationSpeed*Time.fixedDeltaTime,0);

            /*_direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _animation.SetDirection(_direction.x, _direction.z);*/
        }

        private void FixedUpdate()
        {
            Move();
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

        private void Move()
        {
            if (_direction == Vector3.zero)
                return;

            var motion = transform.rotation * Vector3.ClampMagnitude(_moveSpeed * _direction, _moveSpeed) *
                         Time.fixedDeltaTime;

            motion = TryMove(motion);
            
            //var dist = Vector3.Distance()
            //motion.z = Mathf.Clamp();
            /*if (Mathf.Abs(_enemy.position.z - transform.position.z + motion.z) <= _minDist && motion.z > 0)
                motion.z = 0;
            if (Mathf.Abs(_enemy.position.x - transform.position.x + motion.x) <= _minDist && motion.x > 0)
                motion.x = 0;
            if (Mathf.Abs(_enemy.position.x - transform.position.x + motion.x) +
                Mathf.Abs(_enemy.position.z - transform.position.z + motion.z) <= _minDist)
            {
                motion.x = 0;
                motion.z = 0;
            }*/
            
            transform.Translate(motion, Space.World);

            //Debug.Log(Vector3.Distance(transform.position - motion, _enemy.position) + " " + Vector3.Distance(transform.position, _enemy.position));

            Rotate();
            _camera.UpdateCamera();
            
            _direction = Vector3.zero;
        }

        private void Rotate()
        {
            var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            var rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = rotation;
        }
        
        private void SetDirection(Vector3 direction)
        {
            /*var motion = transform.rotation * Vector3.ClampMagnitude(_moveSpeed * direction, _moveSpeed) * Time.fixedDeltaTime;

            if (Mathf.Abs(_enemy.position.z - transform.position.z + motion.z) <= _minDist && direction.z > 0)
               direction.z = 0;*/
            
            _direction = direction;
            _animation.SetDirection(direction.x, direction.z);
        }

        private void StartMove()
        {
            _animation.StartMove();
        }
    
        private void StopMove()
        {
            _animation.StopMove();
        }
        
        private void ConnectActions()
        {
            GameManager.PlayerController.OnDirectionChanged += SetDirection;
            GameManager.PlayerController.OnMoveStarted += StartMove;
            GameManager.PlayerController.OnMoveStopped += StopMove;
        }

        private Vector3 TryMove(Vector3 displacement)
        {
            CapsuleCollider coll = GetComponent<CapsuleCollider>();
            float radius = coll.radius * transform.localScale.x;
            Vector3 center = transform.position + coll.center + displacement;
            Vector3 pos1 = center - new Vector3(0.0f, (coll.height / 2) - radius - 0.1f, 0.0f);
            Vector3 pos2 = center + new Vector3(0.0f, (coll.height / 2) - radius, 0.0f);
            float maxDistance = displacement.magnitude;
            LayerMask mask = LayerMask.GetMask("Enemy");

            Vector3 dir = new Vector3(transform.forward.x, 0.0f, 0.0f);
            foreach (RaycastHit c in Physics.CapsuleCastAll(pos1, pos2, radius, dir, maxDistance, mask))
            {
                if (c.collider != null && !c.collider.CompareTag("Player"))
                {
                    displacement.x = 0;
                }
            }

            dir = new Vector3(0.0f, 0.0f, transform.forward.z);
            foreach (RaycastHit c in Physics.CapsuleCastAll(pos1, pos2, radius, dir, maxDistance, mask))
            {
                if (c.collider != null && !c.collider.CompareTag("Player"))
                {
                    displacement.z = 0;
                }
            }

            return displacement;
        }
    }
}
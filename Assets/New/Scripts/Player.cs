using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace New
{
    [RequireComponent(typeof(AnimationBase),
        typeof(CharacterController),
        typeof(BodyUpSystem))]
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
        [SerializeField] private Hand _leftHand;
        [SerializeField] private Hand _rightHand;
        
        [SerializeField] private bool _testMove;
        [Range(-1, 1)]
        [SerializeField] private float _speedX;
        [Range(-1, 1)]
        [SerializeField] private float _speedZ;

        private void Awake()
        {
            GameManager.Player2 = this;
            _animation = GetComponent<AnimationBase>();
            _character = GetComponent<CharacterController>();
            upSystem = GetComponent<BodyUpSystem>();
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

            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(0, -_rotationSpeed * Time.fixedDeltaTime, 0);
            if (Input.GetKey(KeyCode.E))
                transform.Rotate(0, _rotationSpeed * Time.fixedDeltaTime, 0);

            if (_testMove)
                SetDirection(new Vector3(_speedX, 0, _speedZ));

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = GameManager.Camera2.gameCamera.ScreenPointToRay(Input.mousePosition);

                if (!Physics.Raycast(ray, out hit)) return;
                if (hit.collider.gameObject.TryGetComponent<Body>(out var body))
                {
                    body.upSystem.Hit();
                }
            }
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

            var direction = transform.rotation * _direction;
            var motion = Vector3.ClampMagnitude(_moveSpeed * direction, _moveSpeed) * Time.fixedDeltaTime;
            _character.Move(motion);
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
    }
}
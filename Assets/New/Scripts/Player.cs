using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace New
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : Body
    {
        private CharacterController _character;
        [SerializeField] private HitParticles _hitParticles;
        private Transform _enemy;
        private GameCamera _camera;
        private Vector3 _direction;
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private Hand _leftHand;
        [SerializeField] private Hand _rightHand;

        [SerializeField] private Transform _headTarget;
        [SerializeField] private Transform _legsTarget;

        public Vector3 startPosition;
        
        [SerializeField] private bool _testMove;
        [Range(-1, 1)]
        [SerializeField] private float _speedX;
        [Range(-1, 1)]
        [SerializeField] private float _speedZ;
        
        [SerializeField] private int _damage = 10;
        
        protected override void Awake()
        {
            base.Awake();
            GameManager.Player2 = this;
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
                animationSysem.StartPunch();
            else if (Input.GetKeyUp(KeyCode.Space))
                animationSysem.StopPunch();

            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(0, -_rotationSpeed * Time.fixedDeltaTime, 0);
            if (Input.GetKey(KeyCode.E))
                transform.Rotate(0, _rotationSpeed * Time.fixedDeltaTime, 0);

            if (_testMove)
                SetDirection(new Vector3(_speedX, 0, _speedZ));

            if (Input.GetKeyDown(KeyCode.E))
            {
                ragdollSystem.StartFall(60000f, true);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ragdollSystem.StartStandUp());
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.BattleIsStarted == false)
                    GameManager.StartBattle();
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(GameManager.Camera2.EndBattleCamera());
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void LeftPunch() //TODO: сделать чтобы удар проверялся в апдейте
        {
            if(_leftHand.Body is null)
                return;
            _hitParticles.StartHitParticles(_rightHand.type);
            _leftHand.Body.GetHit(transform.position, _damage);
        }
        
        public void RightPunch()
        {
            if(_rightHand.Body is null)
                return;
            _hitParticles.StartHitParticles(_leftHand.type);
            _rightHand.Body.GetHit(transform.position, _damage);
        }

        public void Win()
        {
            //GameManager.PlayerController.LockInput(true);
            health.Toggle(false);
            animationSysem.StopPunch();
            animationSysem.StopMove();
            animationSysem.StartIdle(true);
            StartCoroutine(GameManager.Camera2.EndBattleCamera());
        }
        
        private void Move()
        {
            if (_direction == Vector3.zero || _character.enabled == false)
                return;

            var direction = transform.rotation * _direction;
            var motion = Vector3.ClampMagnitude(_moveSpeed * direction, _moveSpeed) * Time.fixedDeltaTime;
            _character.Move(motion);
            Rotate();
            _camera.UpdateCamera();
            _direction = Vector3.zero;
        }

        public void Rotate()
        {
            var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            var rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = rotation;
            
            /*rotation = Quaternion.Lerp(_headTarget.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            _headTarget.rotation = rotation;*/
            
            targetRotation = Quaternion.LookRotation(_direction, Vector3.up);
            rotation = Quaternion.Lerp(_legsTarget.localRotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            //_legsTarget.localRotation = rotation;
            
            /*var angle = rotation.eulerAngles.y;
            Debug.Log(angle);
            _legsTarget.localRotation *= rotation;*/
            var angle = rotation.eulerAngles.y;
            Debug.Log(rotation.eulerAngles);
            if ((angle > 0 && angle > 270) || (angle > 0 && angle < 90))
            {
                _legsTarget.localRotation = rotation;
            }
            else
            {
                rotation = Quaternion.Slerp(_legsTarget.localRotation, Quaternion.Euler(Vector3.zero), _rotationSpeed * Time.fixedDeltaTime);
                _legsTarget.localRotation = rotation;
            }
        }

        private void SetDirection(Vector3 direction)
        {
            _direction = direction;
            animationSysem.SetDirection(direction.x, direction.z);
        }

        private void StartMove()
        {
            animationSysem.StartMove();
        }
    
        private void StopMove()
        {
            animationSysem.StopMove();
        }

        protected override void Die()
        {
            base.Die();
            GameManager.PlayerController.LockInput(true);
            GameManager.Enemy2.Win();
            ragdollSystem.StartFall(25000f);
        }
        
        protected override void ConnectActions()
        {
            base.ConnectActions();
            GameManager.PlayerController.OnDirectionChanged += SetDirection;
            GameManager.PlayerController.OnMoveStarted += StartMove;
            GameManager.PlayerController.OnMoveStopped += StopMove;
        }
    }
}
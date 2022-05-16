using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
        [SerializeField] private MultiRotationConstraint _legsConst;
        [SerializeField] private MultiRotationConstraint _headConst;

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
            //if(_direction == Vector3.zero)
             //   _camera.UpdateCamera();
        }

        public void LeftPunch() //TODO: сделать чтобы удар проверялся в апдейте
        {
            Punch(_leftHand);
        }
        
        public void RightPunch()
        {
            Punch(_rightHand);
        }

        private void Punch(Hand hand)
        {
            if(hand.Body is null)
                return;
            _hitParticles.StartHitParticles(hand.type);
            hand.Body.GetHit(transform.position, _damage);
            if (hand.Body.IsLastHit(_damage))
            {
                finishPunch = true;
            }
        }
        
        public void FinishPunch()
        {
            Punch(_rightHand);
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
            var rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            
            transform.rotation = rotation;

            if(_direction == Vector3.zero)
                return;
            
            //_headTarget.localRotation = rotation;
            
            targetRotation = Quaternion.LookRotation(_direction, Vector3.up);
            rotation = Quaternion.Lerp(_legsTarget.localRotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            
            var angle = rotation.eulerAngles.y;
            
            if (angle < 270 && angle > 90)
            {
                targetRotation = Quaternion.LookRotation(-_direction, Vector3.up);// * Quaternion.Euler(new Vector3(0, 180, 0));//180 - angle
                rotation = Quaternion.Lerp(_legsTarget.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            }
            
            /*else
            {
                _headTarget.rotation = transform.rotation;
            }*/
            
            _legsTarget.localRotation = rotation;
        }

        private void ResetRotation()
        {
            /*var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = targetRotation;*/
            _legsTarget.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            _headTarget.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            _legsConst.data.offset = new Vector3(0, 35f, 0);
            _headConst.data.offset = new Vector3(0, 35f, 0);
        }
        
        private void SetDirection(Vector3 direction)
        {
            _direction = direction;
            animationSysem.SetDirection(direction.x, direction.z);
        }

        private void StartMove()
        {
            _legsConst.data.offset = Vector3.zero;
            //_headConst.data.offset = Vector3.zero;
            animationSysem.StartMove();
        }
    
        private void StopMove()
        {
            ResetRotation();
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
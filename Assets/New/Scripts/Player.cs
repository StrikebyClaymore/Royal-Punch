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
        typeof(CapsuleCollider))]
    public class Player : Body
    {
        private CharacterController _character;
        private AnimationBase _animation;
        private CapsuleCollider _collider;
        [SerializeField] private HitParticles _hitParticles;
        private Transform _enemy;
        private GameCamera _camera;
        private Vector3 _direction;
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _minDist;
        [SerializeField] private Hand _leftHand;
        [SerializeField] private Hand _rightHand;

        private Vector3 _displacement;
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
            _collider = GetComponent<CapsuleCollider>();
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

            if (_testMove)
                SetDirection(new Vector3(_speedX, 0, _speedZ));
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
            
            //transform.Translate(motion, Space.World);
            _character.Move(motion);
            //_character.SimpleMove(Vector3.ClampMagnitude(_moveSpeed * direction, _moveSpeed));
            //К примеру если встать вплотную к таргету и жазать вбок бежать
            //Debug.Log(motion + " " + Vector3.Distance(transform.position - motion, _enemy.position) + " " + Vector3.Distance(transform.position, _enemy.position));

            Rotate();
            _camera.UpdateCamera();
            
            _direction = Vector3.zero;

            _displacement = motion;
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
            var motion = transform.rotation * Vector3.ClampMagnitude(_moveSpeed * direction, _moveSpeed) * Time.fixedDeltaTime;

            /*if (Mathf.Abs(_enemy.position.z - transform.position.z + motion.z) <= _minDist && direction.z > 0)
               direction.z = 0;
            
            if (Vector3.Distance(transform.position + (_moveSpeed * Time.fixedDeltaTime * direction), _enemy.position) < _minDist && direction.z > 0)
                direction.z = 0;*/

            /*if (direction.z > 0 && TryMove(motion) == false)
                direction.z = 0;*/
            
            //Debug.Log(direction);
            
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

        private bool TryMove(Vector3 displacement)
        {
            var coll = _collider;
            var radius = coll.radius * transform.localScale.x;
            var center = transform.position + coll.center;
            var pos1 = center + Vector3.up * ((coll.height / 2) * transform.localScale.x);
            var pos2 = center - Vector3.up * ((coll.height / 2) * transform.localScale.x);
            var maxDistance = _minDist;//displacement.magnitude;
            var direction = (_enemy.position - transform.position).normalized;
            //Debug.Log(Physics.Raycast(transform.position, direction, maxDistance, mask));
            return !Physics.Raycast(transform.position, direction, maxDistance); //!Physics.CapsuleCast(pos1, pos2, radius, direction, maxDistance, mask);

            /*CapsuleCollider coll = GetComponent<CapsuleCollider>();
            float radius = coll.radius * transform.localScale.x;
            Vector3 center = transform.position + coll.center + displacement;
            
            /*Vector3 pos1 = center - new Vector3(0.0f, (coll.height / 2) - radius - 0.1f, 0.0f);
            Vector3 pos2 = center + new Vector3(0.0f, (coll.height / 2) - radius, 0.0f);#1#
            var pos1 = center + Vector3.up * (coll.height / 2);
            var pos2 = center - Vector3.up * (coll.height / 2);
            
            float maxDistance = displacement.magnitude;
            LayerMask mask = LayerMask.GetMask("Enemy");

            Vector3 dir = new Vector3(transform.forward.x, 0.0f, 0.0f);
            foreach (RaycastHit c in Physics.CapsuleCastAll(pos1, pos2, radius, dir, maxDistance, mask))
            {
                if (c.collider != null)
                {
                    displacement.x = 0;
                }
            }

            dir = new Vector3(0.0f, 0.0f, transform.forward.z);
            foreach (RaycastHit c in Physics.CapsuleCastAll(pos1, pos2, radius, dir, maxDistance, mask))
            {
                if (c.collider != null)
                {
                    displacement.z = 0;
                }
            }

            return displacement;*/
        }

        private void OnDrawGizmos()
        {
            if(_enemy == null)
                return;

            Gizmos.color = Color.green;
            var direction = (_enemy.position - transform.position).normalized;
            Gizmos.DrawLine(transform.position, transform.position + direction * _minDist);
            
            var coll = _collider;
            var center = transform.position + coll.center + _displacement;
            var radius = coll.radius * transform.localScale.x;
            var pos1 = center + Vector3.up * (coll.height / 2) * transform.localScale.x;
            var pos2 = center - Vector3.up * (coll.height / 2) * transform.localScale.x;
            Gizmos.color = new Color(0.5f, 0.1f, 0.1f, 0.5f);
            Gizmos.DrawSphere(center, radius);
            Gizmos.color = new Color(0.5f, 0.5f, 1f, 1f);
            Gizmos.DrawLine(pos1, pos2);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _displacement * 30f);

            Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
            Gizmos.DrawSphere(_enemy.position, 0.5f*7f);
        }
    }
}
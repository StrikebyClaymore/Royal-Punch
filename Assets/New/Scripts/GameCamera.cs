using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        [HideInInspector] public Camera gameCamera;
        private Transform _player;
        private Transform _enemy;

        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _startRotation;
        [SerializeField] private Vector3 _battlePosition;
        [SerializeField] private Vector3 _battleRotation;
        private Vector3 _startOffset;
        private Vector3 _battleOffset;
        private float _battleRotationOffset;
        
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _moveSpeed = 10f;
        
        [SerializeField] private float _maxStartSpeed;
        [SerializeField] private float _minStartSpeed;
        [SerializeField] private float startSpeed;

        [SerializeField] private Transform _cameraTarget;
        private bool _isRotateToBattle;
        private bool _isRotateToStart;
        private bool _playerIsFall;

        private void Awake()
        {
            gameCamera = GetComponent<Camera>();
            GameManager.Camera2 = this;
        }

        private void Start()
        {
            _player = GameManager.Player2.transform;
            _enemy = GameManager.Enemy2.transform;

            //_cameraTarget = _player;

            _startOffset = _startPosition - _player.position;
            _battleOffset = _battlePosition - _player.position;
            
            /*var relativePos = _enemy.position - _battlePosition;
            var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            _battleRotationOffset = Quaternion.Angle(quaternion.Euler(_battlePosition), rotation);*/
            _battleRotationOffset = Quaternion.Angle(Quaternion.Euler(_battleRotation), _enemy.rotation);//Quaternion.Angle(transform.rotation, GetRotation());

            //transform.position = _startPosition;
            //transform.rotation = Quaternion.Euler(new Vector3(25, 180, 0));
            GameManager.PlayerController.LockInput(false);
            GameManager.BattleIsStarted = true;
            transform.position = _battlePosition;
            transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
            _battleRotationOffset = Quaternion.Angle(transform.rotation, GetRotation());

        }

        private void FixedUpdate()
        {
            if (_isRotateToBattle)
                RotateToBattle();
            else if (_isRotateToStart)
                RotateToStart();
        }

        public void UpdateCamera()
        {
            var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                                 Quaternion.Euler(new Vector3(_battleRotationOffset, 0, 0));
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

            var targetPosition = _cameraTarget.rotation * _battleOffset + _cameraTarget.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
        }
        
        private void RotateToBattle()
        {
            var targetRotation = Quaternion.Euler(_battleRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, startSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _battlePosition, startSpeed * Time.fixedDeltaTime);
            startSpeed = Mathf.Min(_maxStartSpeed, startSpeed + Time.deltaTime);
            //Debug.Log($"{(transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude} {(transform.position  - _battlePosition).magnitude}");
            if (360f - (transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f && (transform.position  - _battlePosition).magnitude < 0.1f)
            {
                //Debug.Log("ROTATED");
                _battleRotationOffset = Quaternion.Angle(transform.rotation, GetRotation());
                _isRotateToBattle = false;
                GameManager.BattleIsStarted = true;
                GameManager.PlayerController.LockInput(false);
            }
        }
        
        private void RotateToStart()
        {
            var targetRotation = Quaternion.Euler(_startRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, startSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _player.position + _startOffset, startSpeed * Time.fixedDeltaTime);
            
            startSpeed = Mathf.Min(_maxStartSpeed, startSpeed + Time.deltaTime);
            if (360f - (transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f && (transform.position - _player.position + _startOffset).magnitude < 0.1f)
            {
                _isRotateToStart = false;
            }
        }
        
        public void StartBattleCamera()
        {
            _isRotateToBattle = true;
            startSpeed = _minStartSpeed;
        }
        
        public void EndBattleCamera()
        {
            GameManager.PlayerController.LockInput(true);
            _isRotateToBattle = false;
            _isRotateToStart = true;
            startSpeed = _minStartSpeed;
        }
        
        public void SetFall(bool isFall)
        {
            _playerIsFall = isFall;
        }

        private Quaternion GetRotation()
        {
            var relativePos = _enemy.position - transform.position;
            return Quaternion.LookRotation(relativePos, Vector3.up);
        }
    }
}
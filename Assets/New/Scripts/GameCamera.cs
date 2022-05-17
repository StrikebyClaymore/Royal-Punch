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
        private bool _isSmoothRotate;
        private Vector3 _velocity = Vector3.zero;
        [SerializeField] private float _smoothTime = 0.3f;
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
            else if (_isSmoothRotate)
                SmoothRotate();
            //else
            //    UpdateCamera();
        }
        
        public void UpdateCamera()
        {
            /*var targetRotation = Quaternion.Euler(new Vector3(_battleRotation.x, _battleRotation.y + _player.rotation.eulerAngles.y, _battleRotation.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _player.position + _player.rotation * _battleOffset, _moveSpeed * Time.deltaTime);*/
            
            var targetPosition = _cameraTarget.rotation * _battleOffset + _cameraTarget.position;
            var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                                 Quaternion.Euler(new Vector3(_battleRotationOffset, 0, 0));

            /*Debug.Log($"{transform.rotation.eulerAngles} {targetRotation.eulerAngles}" +
                      $" {(transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude}" +
                      $" {(transform.position - targetPosition).magnitude}");*/
            
            if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f && (transform.position - targetPosition).magnitude < 0.1f)
                return;
            
            //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);
            transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
        
        private void RotateToBattle()
        {
            //var targetRotation = Quaternion.Euler(_battleRotation);
            var targetRotation = Quaternion.Euler(new Vector3(_battleRotation.x, _battleRotation.y + _player.rotation.eulerAngles.y, _battleRotation.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, startSpeed * Time.fixedDeltaTime);
            //transform.position = Vector3.Lerp(transform.position, _battlePosition, startSpeed * Time.fixedDeltaTime);
            transform.position = Vector3.Lerp(transform.position, _player.position + _player.rotation * _battleOffset, startSpeed * Time.fixedDeltaTime);
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
            var targetRotation = Quaternion.Euler(new Vector3(_startRotation.x, _startRotation.y + _player.rotation.eulerAngles.y, _startRotation.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, startSpeed * Time.fixedDeltaTime);
            transform.position = Vector3.Lerp(transform.position, _player.position + _player.rotation * _startOffset, startSpeed * Time.fixedDeltaTime);
            
            startSpeed = Mathf.Min(_maxStartSpeed, startSpeed + Time.fixedDeltaTime);
            
            if (360f - (transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f && (transform.position - _player.position + _startOffset).magnitude < 0.1f)
            {
                _isRotateToStart = false;
            }
        }

        private void SmoothRotate()
        {
            var targetPosition = _cameraTarget.rotation * _battleOffset + _cameraTarget.position;
            var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                                 Quaternion.Euler(new Vector3(_battleRotationOffset, 0, 0));

           // transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            
            Debug.Log($"{(transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude} {Vector3.Distance(transform.position, targetPosition)}");
            
            startSpeed = Mathf.Min(_maxStartSpeed, startSpeed + Time.fixedDeltaTime);
            
            if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f && Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Debug.Log("STOP ROTATE");
                _isSmoothRotate = false;
            }
        }
        
        public void StartBattleCamera()
        {
            _isRotateToBattle = true;
            startSpeed = _minStartSpeed;
        }
        
        public IEnumerator EndBattleCamera()
        {
            _isRotateToBattle = false;
            yield return new WaitForSeconds(1f);
            _isRotateToStart = true;
            startSpeed = _minStartSpeed;
        }

        public void StartSmoothRotate()
        {
            _isSmoothRotate = true;
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
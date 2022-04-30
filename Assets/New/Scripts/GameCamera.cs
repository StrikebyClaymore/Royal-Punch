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
        private Vector3 _offset;
        private float _rotationOffset;
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _moveSpeed = 10f;

        [SerializeField] private Transform _cameraTarget;
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

            _offset = transform.position - _player.position;
            _rotationOffset = Quaternion.Angle(transform.rotation, GetRotation());
        }

        public void UpdateCamera()
        {

            var _target = _player;
            if (_playerIsFall)
                _target = _cameraTarget;
            
            
            var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                                 Quaternion.Euler(new Vector3(_rotationOffset, 0, 0));
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            

            var targetPosition = _target.rotation * _offset + _target.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
        }

        public void SetFall(bool b)
        {
            _playerIsFall = b;
        }

        private Quaternion GetRotation()
        {
            var relativePos = _enemy.position - transform.position;
            return Quaternion.LookRotation(relativePos, Vector3.up);
        }
    }
}
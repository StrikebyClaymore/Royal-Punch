using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [HideInInspector]
    public Camera gameCamera;
    private Transform _player;
    private Transform _enemy;
    private Vector3 _offset;
    private float _rotationOffset;
    private float _rotationSpeed = 100f;
    private float _moveSpeed = 100f;

    [SerializeField] private Transform _cameraTarget;
    private bool _playerIsFall;
    private Transform _target;
    
    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        GameManager.Camera = this;
    }

    private void Start()
    {
        _player = GameManager.Player.transform;
        _enemy = GameManager.Enemy.transform;

        _target = _player;
        
        _offset = transform.position - _target.position;
        _rotationOffset = Quaternion.Angle(transform.rotation, GetRotation());
    }

    private void FixedUpdate()
    {
        /*transform.LookAt(_enemy);
        transform.position = _player.rotation * _offset + _player.position;*/

        if (_target != _cameraTarget)
        {

            var relativePos = _enemy.position - transform.position;
            var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                                 Quaternion.Euler(new Vector3(_rotationOffset, 0, 0));
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        var targetPosition = _player.rotation * _offset + _target.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
    }

    public void ChangeTarget()
    {
        _target = _target == _cameraTarget ? _player : _cameraTarget;
    }
    
    private Quaternion GetRotation()
    {
        var relativePos = _enemy.position - transform.position;
        return Quaternion.LookRotation(relativePos, Vector3.up);
    }
}
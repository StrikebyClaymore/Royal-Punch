using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator),
    typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimator _animator;
    private Rigidbody _rb;
    private Transform _enemy;

    private const float MoveSpeed = 8f;
    private const float RotationSpeed = 100f;
    private Vector3 _direction;
    private bool _lockForwardMovement;
    
    private void Awake()
    {
        _animator = GetComponent<PlayerAnimator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ConnectActions();
        _enemy = GameManager.Enemy.transform;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void LockForwardMovement(bool _lock)
    {
        _lockForwardMovement = _lock;
    }
    
    private void Move()
    {
        if(_direction == Vector3.zero)
            return;
        var motion = transform.localRotation * _direction * (MoveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + motion);
        _direction = Vector3.zero;
        
        Rotate();
    }

    private void Rotate()
    {
        var relativePos = _enemy.position - _rb.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        var rotation = Quaternion.Lerp(_rb.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);
        _rb.rotation = rotation;
    }

    private void SetDirection(Vector3 direction)
    {
        if (_lockForwardMovement && direction.z > 0)
            direction.z = 0;
        _direction = direction;
        _animator.SetRunDirection(direction);
    }

    private void StartMove()
    {
        _animator.SetRun();
    }
    
    private void StopMove()
    {
        _animator.SetIdle();
    }

    private void ConnectActions()
    {
        GameManager.PlayerController.OnDirectionChanged += SetDirection;
        GameManager.PlayerController.OnMoveStarted += StartMove;
        GameManager.PlayerController.OnMoveStopped += StopMove;
    }
}

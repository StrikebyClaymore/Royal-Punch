using System;
using UnityEngine;

public class EnemyAttack : BaseAttack
{
    private Transform _player;
    [SerializeField] private float _rotationSpeed = 1f;
    
    private void Start()
    {
        base.ConnectActions();
        _player = GameManager.Player.transform;
    }
    
    private void LateUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        var relativePos = _player.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        var rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = rotation;
    }
}
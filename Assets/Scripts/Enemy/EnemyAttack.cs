using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : BaseAttack
{
    private Transform _player;
    [SerializeField] private SuperAttackChargeEffect _chargeEffect;
    [SerializeField] private int _superAttackDamage = 100;
    private const float _superAttackChargingTime = 0.4f;
    
    private void Start()
    {
        _player = GameManager.Player.transform;
        StartSuperAttackCharging();
    }

    private void FixedUpdate()
    {
        transform.LookAt(_player);
    }

    private void StartSuperAttackCharging()
    {
        ((EnemyAnimator) animator).SetSuperAttack();
        StartCoroutine(StartSuperCharge());
    }

    private IEnumerator StartSuperCharge()
    {
        yield return new WaitForSeconds(_superAttackChargingTime);
        animator.SetSpeed(0);
        _chargeEffect.StartCharging();
    }
    
    private void StartSuperAttack()
    {
        animator.SetSpeed(1);
        ((EnemyAnimator) animator).ContinueSuperAttack(_superAttackChargingTime);
    }

    private void SuperAttack()
    {
        if(_chargeEffect.target != null)
            _chargeEffect.target.hitArea.Hit(_superAttackDamage, Vector3.zero);
        _chargeEffect.gameObject.SetActive(false);
        animator.SetIdle();
    }
    
    protected override void ConnectActions()
    {
        base.ConnectActions();
        _chargeEffect.OnCharged += StartSuperAttack;
        ((EnemyAnimator) animator).OnSuperAttackCompleted += SuperAttack;
    }
}

using UnityEngine;

public class Enemy : Boxer
{
    private EnemyAttack _attack;
    
    protected override void Awake()
    {
        base.Awake();
        GameManager.Enemy = this;
        _attack = GetComponent<EnemyAttack>();
    }

    private void Start()
    {
       ConnectActions();
    }

    private void StartBattle()
    {
        health.Toggle(true);
    }
    
    protected override void Die()
    {
        base.Die();
        _attack.CancelSuperAttack();
    }

    protected override void ConnectActions()
    {
        base.ConnectActions();
        GameManager.Camera.OnBattleStarting += StartBattle;
    }
}
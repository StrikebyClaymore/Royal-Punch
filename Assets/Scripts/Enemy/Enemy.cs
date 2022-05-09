using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Body
{
    protected override void Awake()
    {
        base.Awake();
        GameManager.Enemy = this;
    }
    
    private void Start()
    {
        ragdollStateChanger.Off();
    }

    public override void Win()
    {
        base.Win();
        animator.SetIdle();
    }
    
    protected override void Die()
    {
        base.Die();
        GameManager.PlayerController.LockInput(true);
        GameManager.Player.Win();
    }
}

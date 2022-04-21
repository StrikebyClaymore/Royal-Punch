using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Body
{
    protected override void Awake()
    {
        base.Awake();
        GameManager.Player = this;
    }
    
    private void Start()
    {
        ragdollStateChanger.Off();
    }

    public override void Win()
    {
        base.Win();
        ((PlayerAnimator) animator).SetWin();
    }
    
    protected override void Die()
    {
        base.Die();
        GameManager.Camera.ChangeTarget();
        GameManager.PlayerController.ToggleInput();
        GameManager.Enemy.Win();
    }
}

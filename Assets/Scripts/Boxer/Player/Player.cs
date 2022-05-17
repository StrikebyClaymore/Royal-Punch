using System;
using UnityEngine;

public class Player : Boxer
{
    protected internal PlayerAnimation animationSystem;
    protected internal PlayerMomement movement;

    protected override void Awake()
    {
        base.Awake();
        animationSystem = GetComponent<PlayerAnimation>();
        movement = GetComponent<PlayerMomement>();
        GameManager.Player = this;
    }

    private void Start()
    {
        base.ConnectActions();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ragdollSystem.KnockOut(25000f);
        }
    }
}
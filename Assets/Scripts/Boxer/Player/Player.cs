using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Boxer
{
    protected internal new PlayerAnimation animationSystem;
    protected internal PlayerMomement movement;
    protected internal PlayerAttack _attack;
    
    private const float EndDelay = 0.3f;
    private const float EndBattleDelay = 80f;

    protected override void Awake()
    {
        base.Awake();
        animationSystem = GetComponent<PlayerAnimation>();
        movement = GetComponent<PlayerMomement>();
        _attack = GetComponent<PlayerAttack>();
        GameManager.Player = this;
    }

    private void Start()
    {
        ConnectActions();
        animationSystem.PlayStartAnim();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ragdollSystem.KnockOut(25000f, true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public IEnumerator Win()
    {
        GameManager.Win = true;
        GameManager.GameData.level++;
        GameManager.PlayerController.LockInput(true, true);
        health.Toggle(false);
        yield return new WaitForSeconds(EndDelay);
        movement.StopConst();
        animationSystem.StartWin();
        animationSystem.OnAnimationCompleted += WinCompleted;
        animationSystem.AddAnimationCompletedEvent(0, EndBattleDelay);
        GameManager.SaluteEffect.StartSalute();
    }

    private void WinCompleted()
    {
        GameManager.Camera.StartEndBattleCamera();
        GameManager.RootMenu.ChangeController(RootMenu.ControllerTypes.End);
    }
    
    public IEnumerator Lose()
    {
        GameManager.Win = false;
        GameManager.PlayerController.LockInput(true, true);
        yield return new WaitForSeconds(EndDelay);
        GameManager.RootMenu.ChangeController(RootMenu.ControllerTypes.End);
    }

    private void StartBattle()
    {
        ApplyUpgrades();
        health.Toggle(true);
        animationSystem.StartIdle(true);
        GameManager.BattleIsStarted = true;
        GameManager.PlayerController.LockInput(false);
    }

    protected override void ApplyUpgrades()
    {
        var level = GameManager.GameData.health;
        var newHealth = config.defaultHealth;
        var addCost = config.startAddHealth;
        for (int i = 2; i <= level; i++)
        {
            newHealth += addCost;
            addCost += config.upAddHealth;
        }
        health.SetMaxHealth(newHealth);
        
        level = GameManager.GameData.strength;
        var newDamage = config.defaultDamage;
        var addDamage = config.startAddDamage;
        for (int i = 2; i <= level; i++)
        {
            newDamage += addDamage;
            addDamage += config.upAddDamage;
        }
        _attack.SetDamage(newDamage);
    }

    protected internal override void Lock()
    {
        base.Lock();
        GameManager.PlayerController.LockInput(true);
    }

    protected override void ConnectActions()
    {
        base.ConnectActions();
        GameManager.Camera.OnBattleStarting += StartBattle;
    }
}
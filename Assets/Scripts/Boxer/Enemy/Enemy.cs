using UnityEngine;

public class Enemy : Boxer
{
    private EnemyAttack _attack;
    protected internal AnimationRigging animationRigging;

    protected override void Awake()
    {
        base.Awake();
        GameManager.Enemy = this;
        _attack = GetComponent<EnemyAttack>();
        animationRigging = GetComponent<AnimationRigging>();
    }

    private void Start()
    {
       ConnectActions();
    }

    public override void GetHit(Vector3 hitPoint, int damage)
    {
        health.ApplyDamage(damage);
        animationRigging.AddHitReaction(hitPoint);
    }
    
    public int GetDestroyedHp() => health.GetDestroyedHp();
    
    private void StartBattle()
    {
        ApplyUpgrades();
        health.Toggle(true);
    }

    protected override void ApplyUpgrades()
    {
        var level = GameManager.GameData.level;
        var newHealth = config.defaultHealth;
        var addHealth = config.startAddHealth;
        for (int i = 2; i <= level; i++)
        {
            newHealth += addHealth;
            addHealth += config.upAddHealth;
        }
        health.SetMaxHealth(newHealth);
        
        var newDamage = config.defaultDamage;
        var addDamage = config.startAddDamage;
        for (int i = 2; i <= level; i++)
        {
            newDamage += addDamage;
            addDamage += config.upAddDamage;
        }
        _attack.SetDamage(newDamage);
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
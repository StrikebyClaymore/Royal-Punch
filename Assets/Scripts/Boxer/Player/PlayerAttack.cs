using UnityEngine;

public class PlayerAttack : BaseAttack
{
    [SerializeField] private HitParticles _hitParticles;
    [SerializeField] protected internal ComboEffect comboEffect;
    
    private void Start()
    {
        ConnectActions();
    }
    
    protected override void Punch(Hand hand)
    {
        if (hand.Body == null)
            return;

        var damageWithCombo = (int)(damage * comboEffect.GetComboCount() * boxer.config.comboDamageMultiplier);

        if(finishPunch)
            hand.Body.KnockOut(knockOutForce, damageWithCombo);
        else
        {
            hand.Body.GetHit(transform.position, damageWithCombo);
            comboEffect.IncreaseCombo();
            _hitParticles.StartHitParticles(hand.type);
        }

        if (!hand.Body.IsLastHit(damageWithCombo))
            return;
        
        finishPunch = true;
        StartCoroutine(CheckLock());
    }

    private void  StandUp() => attackRangeDetector.CastTrigger();
    
    protected override void ConnectActions()
    {
        base.ConnectActions();
        ((PlayerRagdollSystem) boxer.ragdollSystem).OnStandUp += StandUp;
    }
}
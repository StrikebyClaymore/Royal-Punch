using UnityEngine;

public class PlayerAttack : BaseAttack
{
    [SerializeField] private HitParticles _hitParticles;
    
    protected override void Punch(Hand hand)
    {
        if(hand.Body is null)
            return;
        base.Punch(hand);
        _hitParticles.StartHitParticles(hand.type);
    }
}
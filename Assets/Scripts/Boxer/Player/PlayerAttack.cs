using UnityEngine;

public class PlayerAttack : BaseAttack
{
    [SerializeField] private HitParticles _hitParticles;
    
    private void Start()
    {
        base.ConnectActions();
    }
    
    protected override void Punch(Hand hand)
    {
        if(hand.Body is null)
            return;
        base.Punch(hand);
        _hitParticles.StartHitParticles(hand.type);
    }
}
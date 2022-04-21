
using UnityEngine;

public class HitArea : Area
{
    [SerializeField] private Health _health;
    [SerializeField] private Rigidbody _hitRb;

    //[SerializeField] private Animator _animator;
 
    // create switch attack hands
    
    public void Hit(int damage, Vector3 force)
    {
        _health.ApplyDamage(damage);
        _hitRb.AddForce(force);
    }
}

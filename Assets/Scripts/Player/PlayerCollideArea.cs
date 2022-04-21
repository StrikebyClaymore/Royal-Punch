
using UnityEngine;

public class PlayerCollideArea : Area
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerAttack _attack;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            _movement.LockForwardMovement(true);
            _attack.StartAttack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            _movement.LockForwardMovement(false);
            _attack.StopAttack();
        }
    }
}
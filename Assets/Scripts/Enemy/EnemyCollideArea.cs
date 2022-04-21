using UnityEngine;

public class EnemyCollideArea : Area
{
    [SerializeField] private EnemyAttack _attack;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var enemy))
        {
            _attack.StartAttack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var enemy))
        {
            _attack.StopAttack();
        }
    }
}
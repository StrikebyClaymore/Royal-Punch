using System;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    public Action OnTargetEnterRange;
    public Action OnTargetExitRange;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHitable>(out var body))
        {
            OnTargetEnterRange?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHitable>(out var body))
        {
            OnTargetExitRange?.Invoke();
        }
    }
}
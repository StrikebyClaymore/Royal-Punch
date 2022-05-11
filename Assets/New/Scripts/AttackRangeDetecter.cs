using System;
using System.Collections;
using System.Collections.Generic;
using New;
using UnityEngine;

public class AttackRangeDetecter : MonoBehaviour
{
    public Action OnTargetEnterRange;
    public Action OnTargetExitRange;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHitable>(out var target))
        {
            OnTargetEnterRange?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHitable>(out var target))
        {
            OnTargetExitRange?.Invoke();
        }
    }
}

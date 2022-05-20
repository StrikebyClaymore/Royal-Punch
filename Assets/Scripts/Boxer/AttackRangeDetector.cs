using System;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    private SphereCollider _collider;
    
    public Action OnTargetEnterRange;
    public Action OnTargetExitRange;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    public void CastTrigger()
    {
        var position = transform.position + _collider.center;
        var hits = Physics.SphereCastAll(position, _collider.radius, Vector3.up, 0, gameObject.layer);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<IHitable>(out var body))
            {
                OnTargetEnterRange?.Invoke();
            }
        }
        OnTargetExitRange?.Invoke();
    }
    
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
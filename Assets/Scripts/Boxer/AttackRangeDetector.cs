using System;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    [SerializeField] private Boxer _boxer;
    private SphereCollider _collider;
    private IHitable _body;
    [SerializeField] private LayerMask _collideLayers;
    
    public Action OnTargetEnterRange;
    public Action OnTargetExitRange;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    public void CastTrigger()
    {
        var position = transform.position + _collider.center;
        var hits = Physics.SphereCastAll(position, _collider.radius * _boxer.transform.localScale.x, transform.forward,
            0, _collideLayers);

        //var o = transform.forward * (_collider.radius * _boxer.transform.localScale.x);
        //Debug.DrawLine(position, position + o, Color.red, 10f);
        
        foreach (var hit in hits)
        {
            if (hit.collider.isTrigger == false && hit.collider.gameObject.TryGetComponent<IHitable>(out var body))
            {
                _body = body;
                OnTargetEnterRange?.Invoke();
                return;
            }
        }
        
        if (_body != null)
        {
            _body = null;
            OnTargetExitRange?.Invoke();   
        }
    }

    private void OnDrawGizmos()
    {
        /*if(_collider == null)
            return;
        Gizmos.color = new Color(0.9f, 0.1f, 0.1f, 0.5f);
        var position = transform.position + _collider.center;
        Gizmos.DrawSphere(position, _collider.radius * _boxer.transform.localScale.x);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHitable>(out var body))
        {
            _body = body;
            OnTargetEnterRange?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHitable>(out var body))
        {
            _body = null;
            OnTargetExitRange?.Invoke();
        }
    }
}
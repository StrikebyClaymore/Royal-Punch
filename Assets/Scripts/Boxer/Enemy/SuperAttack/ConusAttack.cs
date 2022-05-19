using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ConusAttack : SuperAttack
{
    private BoxCollider _collider;
    [SerializeField] private GameObject _areaEffect;
    [SerializeField] private ParticleSystem _smokeEffect;
    [SerializeField] private Vector3 _startColliderCenter;
    [SerializeField] private Vector3 _startColliderSize;
    [SerializeField] private Vector3 _endColliderCenter;
    [SerializeField] private Vector3 _endColliderSize;

    private void Awake()
    {
        _collider = attackCollider as BoxCollider;
    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);
        _areaEffect.SetActive(active);
    }

    public override void SetColliderActive()
    {
        base.SetColliderActive();
        _smokeEffect.Play();
    }
    
    protected override void ColliderProcess()
    {
        var center = new Vector3(_endColliderCenter.x, _endColliderCenter.y, _endColliderCenter.z * nearPlane);
        var size = new Vector3(_endColliderSize.x * nearPlane, _endColliderSize.y, _endColliderSize.z);
        _collider.center = center;
        _collider.size = size;
    }

    protected override void ResetCollider()
    {
        _collider.center = _startColliderCenter;
        _collider.size = _startColliderSize;
    }
}
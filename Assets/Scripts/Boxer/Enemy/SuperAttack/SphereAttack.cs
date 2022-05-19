using UnityEngine;

public class SphereAttack : SuperAttack
{
    private SphereCollider _collider;
    [SerializeField] private GameObject _areaEffect;
    [SerializeField] private ParticleSystem _smokeEffect;
    [SerializeField] private float _startRadius;
    [SerializeField] private float _endRadius;
    
    private void Awake()
    {
        _collider = attackCollider as SphereCollider;
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
        var radius = _endRadius * nearPlane;
        _collider.radius = radius;
    }

    protected override void ResetCollider()
    {
        _collider.radius = _startRadius;
    }
}
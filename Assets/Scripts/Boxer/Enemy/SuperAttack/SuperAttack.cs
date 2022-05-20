using System;
using UnityEngine;

public abstract class SuperAttack : MonoBehaviour
{
    public int id;
    [SerializeField] private Material _material;
    [SerializeField] protected Collider attackCollider;
    [Range(0, 1)]
    [SerializeField] private float _startNearPlane;
    [Range(0, 1)]
    [SerializeField] private float _endNearPlane;
    [Range(0, 1)]
    [SerializeField] private float _startFarPlane;
    [Range(0, 1)]
    [SerializeField] private float _endFarPlane;
    public float chargingTime;
    private readonly int _nearPlane = Shader.PropertyToID("_NearPlane");
    private readonly int _farPlane = Shader.PropertyToID("_FarPlane");
    protected float nearPlane;
    protected float farPlane;
    [SerializeField] protected float processSpeed = 1f;
    [SerializeField] protected float force = 25000f;
    [SerializeField] protected int damage = 20;
    [SerializeField] protected float damageMultiply = 1.5f;

    public Action OnAttack;

    private void Start()
    {
        ResetParams();
    }

    public virtual void SetActive(bool active)
    {
        enabled = active;
    }
    
    public virtual void SetColliderActive()
    {
        attackCollider.enabled = true;
    }
    
    public void SetDamage(int value)
    {
        damage = (int) (value * damageMultiply);
    }
    
    public virtual void Process(EnemyAttack.SuperStates state)
    {
        if (state == EnemyAttack.SuperStates.Charge)
        {
            nearPlane = Mathf.Min(_endNearPlane, nearPlane + Time.deltaTime);
            farPlane = Mathf.Min(_endFarPlane, farPlane + Time.deltaTime);
        }
        else if (state == EnemyAttack.SuperStates.Attack)
        {
            nearPlane = Mathf.Min(1, nearPlane + processSpeed * Time.deltaTime);

            ColliderProcess();

            if (nearPlane == 1)
            {
                OnAttack?.Invoke();
                ResetParams();
            }
        }
        _material.SetFloat(_nearPlane, nearPlane);
        _material.SetFloat(_farPlane, farPlane);
    }

    protected abstract void ColliderProcess();

    private void ResetParams()
    {
        nearPlane = _startNearPlane;
        farPlane = _startFarPlane;
        _material.SetFloat(_nearPlane, _startNearPlane);
        _material.SetFloat(_farPlane, _startFarPlane);
        SetActive(false);
        ResetCollider();
    }
    
    protected abstract void ResetCollider();

    private void OnTriggerEnter(Collider other)
    {
        if(enabled && other.gameObject.TryGetComponent<IHitable>(out var player))
        {
            attackCollider.enabled = false;
            player.KnockOut(force, damage);
        }
    }
}
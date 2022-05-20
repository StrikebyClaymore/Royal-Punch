using UnityEngine;

public abstract class RagdollSystem : MonoBehaviour
{
    protected BaseAnimation animation;
    protected CharacterController character;
    protected CapsuleCollider hitCollider;
    protected GameCamera gameCamera;

    [SerializeField] private Transform _armature;
    protected Rigidbody[] bones;
    private Collider[] _colliders;
    [SerializeField] protected Rigidbody chestRb;
    
    [HideInInspector]
    public bool isActive;
    
    protected virtual void Awake()
    {
        animation = GetComponent<BaseAnimation>();
        character = GetComponent<CharacterController>();
        hitCollider = GetComponent<CapsuleCollider>();
        bones = _armature.GetComponentsInChildren<Rigidbody>();
        _colliders = _armature.GetComponentsInChildren<Collider>();
    }

    public virtual void KnockOut(float force, bool standUp = false) { }
    
    protected void Toggle(bool enable)
    {
        foreach (var rb in bones)
            rb.isKinematic = !enable;
        foreach (var c in _colliders)
            c.isTrigger = !enable;
        isActive = enable;
    }
}
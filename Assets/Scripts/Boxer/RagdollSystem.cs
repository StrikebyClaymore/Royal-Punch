using UnityEngine;
using UnityEngine.UIElements;

public abstract class RagdollSystem : MonoBehaviour
{
    private BaseAnimation _animation;
    private CharacterController _character;
    protected GameCamera gameCamera;
    private Boxer _boxer;

    [SerializeField] private Transform _armature;
    private Rigidbody[] _bones;
    private Collider[] _colliders;
    
    [HideInInspector]
    public bool isActive;
    
    private void Awake()
    {
        _animation = GetComponent<BaseAnimation>();
        _character = GetComponent<CharacterController>();
        _boxer = GetComponent<Boxer>();
        _bones = _armature.GetComponentsInChildren<Rigidbody>();
        _colliders = _armature.GetComponentsInChildren<Collider>();
    }

    protected void Toggle(bool enable)
    {
        foreach (var rb in _bones)
            rb.isKinematic = !enable;
        foreach (var c in _colliders)
            c.isTrigger = !enable;
        isActive = enable;
    }
}
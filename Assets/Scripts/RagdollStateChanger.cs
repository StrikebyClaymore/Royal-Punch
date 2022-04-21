using UnityEngine;

[RequireComponent(typeof(BodyAnimator))]
public class RagdollStateChanger : MonoBehaviour
{
    private BodyAnimator _animator;
    private Rigidbody[] _rigidbodies;

    private void Awake()
    {
        _animator = GetComponent<BodyAnimator>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
    }

    public void On()
    {
        _animator.Off();
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    public void Off()
    {
        _animator.On();
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = true;
        }
    }
}

using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRigging : MonoBehaviour
{
    [SerializeField] private ChainIKConstraint headConstraint;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetBasePosition;
    [SerializeField] private float TargetOffset = 1f; 
    [SerializeField] private float ReturnSpeed = 100f;

    [SerializeField] private Transform _head;
    [SerializeField] private Vector3 _targetOffset;
    [SerializeField] private float _triedTargetOffsetZ;
    private bool _tried;
    
    private void Awake()
    {
        //_targetBasePosition = _target.position;
        _targetOffset = _target.position - _head.position;
    }

    private void FixedUpdate()
    {
        _targetBasePosition = _tried == false ? _head.position + _targetOffset : _head.position + transform.forward * _triedTargetOffsetZ;
        if(Vector3.Distance(_target.position, _targetBasePosition) < ReturnSpeed * Time.fixedDeltaTime)
            return;
        var direction = (_target.position - _targetBasePosition).normalized;
        direction.y = 0;
        var transition = (ReturnSpeed * Time.fixedDeltaTime * direction);
        _target.Translate(-transition, Space.World);
    }
    
    public void AddHitReaction(Vector3 hitPoint)
    {
        var direction = (transform.position - hitPoint).normalized;
        direction.y = 0;
        _target.Translate(direction * TargetOffset, Space.World);
    }

    public void ToggleTried(bool enable) =>_tried = enable;

    public void SetWeight(int weight)
    {
        headConstraint.weight = weight;
    }
}
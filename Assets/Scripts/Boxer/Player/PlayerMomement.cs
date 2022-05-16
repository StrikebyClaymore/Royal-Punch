using UnityEngine;
using UnityEngine.Animations.Rigging;using UnityEngine.UIElements;

[RequireComponent(typeof(Player),
    typeof(CharacterController))]
public class PlayerMomement : MonoBehaviour
{
    private Player _player;
    private CharacterController _character;
    private Transform _enemy;
    private GameCamera _camera;
    [SerializeField] private Transform _headTarget;
    [SerializeField] private Transform _legsTarget;
    [SerializeField] private MultiRotationConstraint _headConst;
    [SerializeField] private MultiRotationConstraint _legsConst;
    private Vector3 _direction;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _moveSpeed = 8f;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _character = GetComponent<CharacterController>();
    }
    
    private void Start()
    {
        _enemy = GameManager.Enemy.transform;
        _camera = GameManager.Camera;
        ConnectActions();
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    {
        if (_direction == Vector3.zero || _character.enabled == false)
            return;

        var direction = transform.rotation * _direction;
        var motion = Vector3.ClampMagnitude(_moveSpeed * direction, _moveSpeed) * Time.fixedDeltaTime;
        _character.Move(motion); 
        Rotate();
        _camera.UpdateCamera();
        _direction = Vector3.zero;
    }
    
    public void Rotate()
    {
        var relativePos = _enemy.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        var rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            
        transform.rotation = rotation;

        if(_direction == Vector3.zero)
            return;

        targetRotation = Quaternion.LookRotation(_direction, Vector3.up);
        rotation = Quaternion.Lerp(_legsTarget.localRotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            
        var angle = rotation.eulerAngles.y;
            
        if (angle < 270 && angle > 90)
        {
            targetRotation = Quaternion.LookRotation(-_direction, Vector3.up);
            rotation = Quaternion.Lerp(_legsTarget.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        _legsTarget.localRotation = rotation;
    }
    
    private void ResetRotation()
    {
        _legsTarget.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        _headTarget.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        _legsConst.data.offset = new Vector3(0, 35f, 0);
    }
    
    private void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _player.animationSystem.SetDirection(direction.x, direction.z);
    }

    private void StartMove()
    {
        _legsConst.data.offset = Vector3.zero;
        _player.animationSystem.StartMove();
    }
    
    private void StopMove()
    {
        ResetRotation();
        _player.animationSystem.StopMove();
    }
    
    private void ConnectActions()
    {
        GameManager.PlayerController.OnDirectionChanged += SetDirection;
        GameManager.PlayerController.OnMoveStarted += StartMove;
        GameManager.PlayerController.OnMoveStopped += StopMove;
    }
}
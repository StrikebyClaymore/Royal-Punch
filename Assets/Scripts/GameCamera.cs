using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    [HideInInspector]
    public Camera gameCamera;
    public Camera uiCamera;
    private Transform _player;
    private Transform _enemy;

    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Vector3 _battlePosition;
    [SerializeField] private Vector3 _battleRotation;
    private Vector3 _startOffset;
    private Vector3 _battleOffset;
    private float _battleRotationOffset;

    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _stopRotationSpeed = 30f;
    [SerializeField] private float _stopMoveSpeed = 10f;

    [SerializeField] private float _maxStartSpeed;
    [SerializeField] private float _minStartSpeed;
    [SerializeField] private float _startSpeed;

    [SerializeField] private Transform _cameraTarget;
    private bool _isRotateToBattle;
    private bool _isRotateToStart;
    private bool _playerIsFall;
    private bool _isStopRotating;
    /*private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _smoothTime = 0.3f;*/

    public Action OnBattleStarting;
    
    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        GameManager.Camera = this;
    }

    private void Start()
    {
        _player = GameManager.Player.transform;
        _enemy = GameManager.Enemy.transform;
        _startOffset = _startPosition - _player.position;
        _battleOffset = _battlePosition - _player.position;

        _battleRotationOffset = Quaternion.Angle(Quaternion.Euler(_battleRotation), _enemy.rotation);

        /*_isRotateToStart = false;
        _isRotateToBattle = false;
        GameManager.BattleIsStarted = true;
        transform.position = _battlePosition;
        transform.rotation = Quaternion.Euler(_battleRotation);
        GameManager.PlayerController.LockInput(false);*/
        
       transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(_startRotation);
        
        _battleRotationOffset = Quaternion.Angle(transform.rotation, GetRotation());
    }

    private void FixedUpdate()
    {
        if (_isRotateToBattle)
            RotateToBattle();
        else if (_isRotateToStart)
            RotateToStart();
        else if (_isStopRotating)
            StopRotating();
    }
    
    public void UpdateCamera()
    {
        var targetPosition = _cameraTarget.rotation * _battleOffset + _cameraTarget.position;
        var relativePos = _enemy.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                             Quaternion.Euler(new Vector3(_battleRotationOffset, 0, 0));

        if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f &&
            (transform.position - targetPosition).magnitude < 0.1f)
            return;

        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    public void StartBattleCamera()
    {
        _isRotateToBattle = true;
        _startSpeed = _minStartSpeed;
        GameManager.RootMenu.ChangeController(RootMenu.ControllerTypes.Battle);
        //GameManager.Player.StartBattle();
    }

    public void StartEndBattleCamera()
    {
        _isRotateToBattle = false;
        _isRotateToStart = true;
        _startSpeed = _minStartSpeed;
    }

    public void StartStopRotate()
    {
        _isStopRotating = true;
    }
    
    public void StartRotate()
    {
        _isStopRotating = false;
    }
    
    public void SetFall(bool isFall) => _playerIsFall = isFall;
    
    private void RotateToBattle()
    {
        var targetRotation = Quaternion.Euler(_battleRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _startSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, _battlePosition, _startSpeed * Time.fixedDeltaTime);
        _startSpeed = Mathf.Min(_maxStartSpeed, _startSpeed + Time.deltaTime);

        if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f &&
            (transform.position - _battlePosition).magnitude < 0.1f)
        {
            _battleRotationOffset = Quaternion.Angle(transform.rotation, GetRotation());
            _isRotateToBattle = false;
            OnBattleStarting?.Invoke();
        }
    }

    private void RotateToStart()
    {
        var targetRotation = Quaternion.Euler(new Vector3(_startRotation.x,
            _startRotation.y + _player.rotation.eulerAngles.y, _startRotation.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _startSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, _player.position + _player.rotation * _startOffset,
            _startSpeed * Time.fixedDeltaTime);

        _startSpeed = Mathf.Min(_maxStartSpeed, _startSpeed + Time.fixedDeltaTime);
        if (360f - (transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f &&
            (transform.position - _player.position + _startOffset).magnitude < 0.1f)
        {
            _isRotateToStart = false;
        }
    }
    
    private void StopRotating()
    {
        var targetPosition = _cameraTarget.rotation * _battleOffset + _cameraTarget.position;
        var relativePos = _enemy.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                             Quaternion.Euler(new Vector3(_battleRotationOffset, 0, 0));
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, _stopMoveSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _stopRotationSpeed * Time.fixedDeltaTime);

        _startSpeed = Mathf.Min(_maxStartSpeed, _startSpeed + Time.fixedDeltaTime);

        if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f && Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            _isStopRotating = false;
        }
    }

    private Quaternion GetRotation()
    {
        var relativePos = _enemy.position - transform.position;
        return Quaternion.LookRotation(relativePos, Vector3.up);
    }
}
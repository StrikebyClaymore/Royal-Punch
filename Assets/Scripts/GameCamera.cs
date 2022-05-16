using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    [HideInInspector]
    public Camera gameCamera;
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

    [SerializeField] private float _maxStartSpeed;
    [SerializeField] private float _minStartSpeed;
    [SerializeField] private float startSpeed;

    [SerializeField] private Transform _cameraTarget;
    private bool _isRotateToBattle;
    private bool _isRotateToStart;
    private bool _playerIsFall;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _endBattleDelayTime = 1f;

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
        
        GameManager.PlayerController.LockInput(false);
        GameManager.BattleIsStarted = true;
        transform.position = _battlePosition;
        transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        _battleRotationOffset = Quaternion.Angle(transform.rotation, GetRotation());

    }

    private void FixedUpdate()
    {
        if (_isRotateToBattle)
            RotateToBattle();
        else if (_isRotateToStart)
            RotateToStart();
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

    private void RotateToBattle()
    {
        var targetRotation = Quaternion.Euler(_battleRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, startSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, _battlePosition, startSpeed * Time.fixedDeltaTime);
        startSpeed = Mathf.Min(_maxStartSpeed, startSpeed + Time.deltaTime);

        if (360f - (transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f &&
            (transform.position - _battlePosition).magnitude < 0.1f)
        {
            _battleRotationOffset = Quaternion.Angle(transform.rotation, GetRotation());
            _isRotateToBattle = false;
            GameManager.BattleIsStarted = true;
            GameManager.PlayerController.LockInput(false);
        }
    }

    private void RotateToStart()
    {
        var targetRotation = Quaternion.Euler(new Vector3(_startRotation.x,
            _startRotation.y + _player.rotation.eulerAngles.y, _startRotation.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, startSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, _player.position + _player.rotation * _startOffset,
            startSpeed * Time.fixedDeltaTime);

        startSpeed = Mathf.Min(_maxStartSpeed, startSpeed + Time.fixedDeltaTime);
        if (360f - (transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f &&
            (transform.position - _player.position + _startOffset).magnitude < 0.1f)
        {
            _isRotateToStart = false;
        }
    }

    public void StartBattleCamera()
    {
        _isRotateToBattle = true;
        startSpeed = _minStartSpeed;
    }

    public IEnumerator EndBattleCamera()
    {
        _isRotateToBattle = false;
        yield return new WaitForSeconds(_endBattleDelayTime);
        _isRotateToStart = true;
        startSpeed = _minStartSpeed;
    }

    public void SetFall(bool isFall) => _playerIsFall = isFall;

    private Quaternion GetRotation()
    {
        var relativePos = _enemy.position - transform.position;
        return Quaternion.LookRotation(relativePos, Vector3.up);
    }
}
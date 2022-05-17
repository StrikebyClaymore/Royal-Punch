using System.Collections;
using UnityEngine;

public class PlayerRagdollSystem : RagdollSystem
{
    private Player _boxer;
    private Quaternion[] rotations;
    [SerializeField] private Transform _hips;
    [SerializeField] private Rigidbody _chestRb;
    [SerializeField] private Transform _cameraTarget;
    private Vector3 _hipsPosition;
    [SerializeField] private float _rotationSpeed = 5;
    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private float _standUpTime;
    [SerializeField] private float _knockOutTime;

    protected override void Awake()
    {
        base.Awake();
        _boxer = GetComponent<Player>();
        rotations = new Quaternion[bones.Length];
    }

    private void Start()
    {
        gameCamera = GameManager.Camera;
        Toggle(false);
        StartCoroutine(SaveRagdollBones());
    }

    private IEnumerator SaveRagdollBones() // Сделать чтобы перед началом раунда чел становился в позу и она сохранялась
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        UpdateRagdollBones();
    }
    
    private void FixedUpdate()
    {
        if (isActive)
        {
            var newPosition = _hips.position;
            newPosition.y = 0f;
            _cameraTarget.position = newPosition;
            _boxer.movement.Rotate();
            gameCamera.UpdateCamera();
        }
        else
        {
            for (var i = 0; i < bones.Length; i++)
            {
                bones[i].transform.localRotation = Quaternion.Slerp(bones[i].transform.localRotation, rotations[i], Time.fixedDeltaTime * _rotationSpeed);
                _hips.position = Vector3.MoveTowards(_hips.position, _hipsPosition, Time.fixedDeltaTime * _movementSpeed);
            }
        }
    }

    public override void KnockOut(float force)
    {
        character.enabled = false;
        _boxer.movement.ResetRotation();
        animation.StopPunch();
        Toggle(true);
        StartCoroutine(ToggleAnimator(0, false));
        _chestRb.velocity = new Vector3(0, 0.02f, 0);
        _chestRb.AddForce(-transform.forward * force);  
        StartCoroutine(StartStandUp());
    }

    private IEnumerator StartStandUp()
    {
        yield return new WaitForSeconds(_knockOutTime);
        Toggle(false);
        _hipsPosition.x = _cameraTarget.position.x;
        _hipsPosition.z = _cameraTarget.position.z;
        StartCoroutine(StandUp());
    }

    private IEnumerator StandUp()
    {
        yield return new WaitForSeconds(_standUpTime);
        animation.Toggle(true);
        animation.StartIdle(true);
        transform.position = _cameraTarget.position + new Vector3(0, 0.5f, 0);
        _cameraTarget.localPosition = Vector3.zero;
        character.enabled = true;
        GameManager.PlayerController.LockInput(false);
    }
    
    private void UpdateRagdollBones()
    {
        _hipsPosition = _cameraTarget.position;
        _hipsPosition.y = _hips.position.y;
        for (int i = 0; i < bones.Length; i++)
            rotations[i] = bones[i].transform.localRotation;
    }
    
    private IEnumerator ToggleAnimator(float time, bool enable)
    {
        yield return new WaitForSeconds(time);
        animation.Toggle(enable);
    }
}
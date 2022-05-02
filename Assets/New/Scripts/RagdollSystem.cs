using System.Collections;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(AnimationBase),
        typeof(CharacterController))]
    public class RagdollSystem : MonoBehaviour
    {
        private AnimationBase _animation;
        private CharacterController _character;
        private GameCamera _camera;
    
        private Rigidbody[] _bones;
        private Quaternion[] rotations;
    
        [SerializeField] private Transform _hips;
        [SerializeField] private Rigidbody _chestRb;
        [SerializeField] private Transform _cameraTarget;
        private Vector3 _hipsPosition;
        [SerializeField] private float _rotationSpeed = 5;
        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private float _standUpTime;
        [HideInInspector]
        public bool isActive;

        private void Awake()
        {
            _animation = GetComponent<AnimationBase>();
            _character = GetComponent<CharacterController>();
            _bones = GetComponentsInChildren<Rigidbody>();
            rotations = new Quaternion[_bones.Length];
        }

        private void Start()
        {
            _camera = GameManager.Camera2;
            Toggle(false);
        }
    
        private void FixedUpdate()
        {
            if (isActive)
            {
                var newPosition = _hips.position;
                newPosition.y = 0f;
                _cameraTarget.position = newPosition;

                _camera.UpdateCamera();
            }
            else
            {
                for (var i = 0; i < _bones.Length; i++)
                {
                    _bones[i].transform.rotation = Quaternion.Lerp(_bones[i].transform.rotation, rotations[i], Time.deltaTime * _rotationSpeed);
                    _hips.position = Vector3.MoveTowards(_hips.position, _hipsPosition, Time.deltaTime * _movementSpeed);
                }
            }
        }
    
        public void StartFall()
        {
            UpdateRagdollBones();
            
            _character.enabled = false;

            Toggle(true);
            
            StartCoroutine(ToggleAnimator(0, false));

            GameManager.Camera2.SetFall(true);
        }
    
        public void StartStandUp()
        {
            Toggle(false);
            
            _hipsPosition.x = _cameraTarget.position.x;
            _hipsPosition.z = _cameraTarget.position.z;
            
            StartCoroutine(ToggleAnimator(_standUpTime, true));
        }
    
        private void UpdateRagdollBones()
        {
            _hipsPosition = _cameraTarget.position;
            _hipsPosition.y = _hips.position.y;
            for (int i = 0; i < _bones.Length; i++)
                rotations[i] = _bones[i].transform.rotation;
        }

        private IEnumerator ToggleAnimator(float time, bool enable)
        {
            yield return new WaitForSeconds(time);

            if(enable == false)
                _chestRb.AddForce(-transform.forward * 25000f);
            else
                transform.position = _cameraTarget.position + new Vector3(0, 0.5f, 0);

            _animation.Toggle(enable);
        }

        private void Toggle(bool enable)
        {
            foreach (var rb in _bones)
                rb.isKinematic = !enable;
            isActive = enable;
        }
    }
}

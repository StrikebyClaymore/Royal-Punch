using System.Collections;
using System.Linq;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(AnimationBase),
        typeof(CharacterController),
        typeof(Body))]
    public class RagdollSystem : MonoBehaviour
    {
        private AnimationBase _animation;
        private CharacterController _character;
        private GameCamera _camera;
        private Body _body;
    
        private Rigidbody[] _bones;
        private Quaternion[] rotations;
        [SerializeField] private Transform _armature;
        private Collider[] _colliders;
        
        [SerializeField] private Transform _hips;
        [SerializeField] private Rigidbody _chestRb;
        [SerializeField] private Transform _cameraTarget;
        private Vector3 _hipsPosition;
        [SerializeField] private float _rotationSpeed = 5;
        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private float _standUpTime;
        [HideInInspector]
        public bool isActive;
        private float _force;

        private void Awake()
        {
            _animation = GetComponent<AnimationBase>();
            _character = GetComponent<CharacterController>();
            _body = GetComponent<Body>();
            _bones = GetComponentsInChildren<Rigidbody>();
            _colliders = _armature.GetComponentsInChildren<Collider>();
            rotations = new Quaternion[_bones.Length];
        }

        private void Start()
        {
            _camera = GameManager.Camera2;
            Toggle(false);
        }
    
        private void FixedUpdate()
        {
            if(_body is Player == false)
                return;
            if (isActive)
            {
                var newPosition = _hips.position;
                newPosition.y = 0f;
                _cameraTarget.position = newPosition;
                
                //((Player)_body).Rotate();
            }
            else
            {
                for (var i = 0; i < _bones.Length; i++)
                {
                    _bones[i].transform.rotation = Quaternion.Lerp(_bones[i].transform.rotation, rotations[i], Time.fixedDeltaTime * _rotationSpeed);
                    _hips.position = Vector3.MoveTowards(_hips.position, _hipsPosition, Time.fixedDeltaTime * _movementSpeed);
                }
            }
        }
    
        public void StartFall(float force, bool standUp = false)
        {
            _force = force;
            
            _animation.StopPunch();

            UpdateRagdollBones();
            
            _character.enabled = false;

            Toggle(true);
            _chestRb.velocity = new Vector3(0, 0.02f, 0);

            StartCoroutine(ToggleAnimator(0, false));
            
            if (standUp)
                StartCoroutine(StartStandUp());

            GameManager.Camera2.SetFall(true);
        }

        public void EnemyStartFall(float force)
        {
            _force = force;
            _character.enabled = false;
            Toggle(true);
            StartCoroutine(ToggleAnimator(0, false));
            GameManager.Player2.Win();
        }

        public IEnumerator StartStandUp()
        {
            yield return new WaitForSeconds(2.0f);
            
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
                _chestRb.AddForce(-transform.forward * _force);
            else
            {
                _animation.StartIdle(true);
                transform.position = _cameraTarget.position + new Vector3(0, 0.5f, 0);
                _cameraTarget.localPosition = Vector3.zero;
                _character.enabled = true;
            }

            _animation.Toggle(enable);
        }

        private bool VelocityIsZero()
        {
            var count = _bones.Count(rb => rb.velocity == Vector3.zero);
            return count == _bones.Length;
        }

        private void Toggle(bool enable)
        {
            foreach (var rb in _bones)
                rb.isKinematic = !enable;
            foreach (var c in _colliders)
                c.isTrigger = !enable;
            isActive = enable;
        }
    }
}

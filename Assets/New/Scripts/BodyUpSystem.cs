using System;
using System.Collections;
using UnityEngine;

namespace New
{
    [RequireComponent(typeof(AnimationBase),
        typeof(CharacterController),
        typeof(RagdollSystem))]
    [RequireComponent(typeof(AnimationRigging),
        typeof(CharacterController))]
    public class BodyUpSystem : MonoBehaviour
    {
        private CharacterController _character;
        private AnimationBase _animation;
        private RagdollSystem _ragdollSystem;
        private AnimationRigging _animationRigging;
        private GameCamera _camera;

        [SerializeField] private Rigidbody _chest;
        [SerializeField] private Transform _cameraTarget;


        private void Awake()
        {
            _animation = GetComponent<AnimationBase>();
            _character = GetComponent<CharacterController>();
            _ragdollSystem = GetComponent<RagdollSystem>();
            _animationRigging = GetComponent<AnimationRigging>();
        }

        private void Start()
        {
            _camera = GameManager.Camera2;
        }

        private void FixedUpdate()
        {
            var newPosition = _chest.transform.position;
            newPosition.y = 0f;
            _cameraTarget.position = newPosition;
            _camera.UpdateCamera();
            //Debug.Log($"{Time.fixedTime} {newPosition}");
        }

        public void Hit()
        {
            enabled = true;
            _character.enabled = false;
            _ragdollSystem.On();
            GameManager.Camera2.SetFall(true);
            _chest.AddForce(-transform.forward * 25000f);

            //StartCoroutine(Up());
        }

        public void Up()
        {
            //yield return new WaitForSeconds(3f);
            enabled = false;
            _ragdollSystem.Off();
            transform.Translate(_cameraTarget.localPosition + new Vector3(0, 0.5f, 0), Space.Self);
            _cameraTarget.localPosition = Vector3.zero;
            //Debug.Log($"{transform.position} {_cameraTarget.position} {_cameraTarget.position + new Vector3(0, 0.5f, 0)}");
            //transform.position = ;
            _character.enabled = true;
            GameManager.Camera2.SetFall(false);
        }
    }
}

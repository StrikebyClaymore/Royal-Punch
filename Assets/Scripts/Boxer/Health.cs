using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Image _background;
    [SerializeField] private Image _foreground;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private int _maxHeath = 100;
    private int _health;

    public Action OnDie;
    
    private void Start()
    {
        _camera = GameManager.Camera.gameCamera;
        _health = _maxHeath;
        UpdateBar();
    }

    public void UpdateRotation()
    {
        //transform.LookAt((-1) * GameManager.Camera.gameCamera.transform.position);
    }
    
    private void LateUpdate()
    {
        transform.LookAt((-1) * GameManager.Camera.gameCamera.transform.position);
        //transform.LookAt((-1) * _camera.transform.position);
        /*var relativePos = _camera.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        
        if (rotation.eulerAngles.magnitude < 8 * Time.fixedDeltaTime)
            return;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, _camera.transform.rotation, 8 * Time.fixedDeltaTime);*/
        
        /*var targetPosition = _cameraTarget.rotation * _battleOffset + _cameraTarget.position;
        var relativePos = _enemy.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                             Quaternion.Euler(new Vector3(_battleRotationOffset, 0, 0));

        /*Debug.Log($"{transform.rotation.eulerAngles} {targetRotation.eulerAngles}" +
                  $" {(transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude}" +
                  $" {(transform.position - targetPosition).magnitude}");#1#
            
        if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f && (transform.position - targetPosition).magnitude < 0.1f)
            return;
            
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
            
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);*/
    }
    
    public void ApplyDamage(int damage)
    {
        _health = Mathf.Max(0, _health - damage);;
        UpdateBar();

        if (_health == 0)
            OnDie?.Invoke();
    }

    public bool IsLastHp(int damage) => _health - damage <= 0;
    
    public void Toggle(bool enable)
    {
        gameObject.SetActive(enable);
        enabled = enable;
    }
    
    private void UpdateBar()
    {
        _foreground.fillAmount = (float) _health / _maxHeath;
        _text.text = _health.ToString();
    }
}

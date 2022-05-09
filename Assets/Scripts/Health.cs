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
        _camera = GameManager.Camera2.gameCamera;
        _health = _maxHeath;
        UpdateBar();
    }

    private void LateUpdate()
    {
        transform.LookAt(-_camera.transform.position);
    }
    
    public void ApplyDamage(int damage)
    {
        _health = Mathf.Max(0, _health - damage);;
        UpdateBar();

        if (_health == 0)
            OnDie?.Invoke();
    }

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

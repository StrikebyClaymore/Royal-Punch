using System;
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

    private void LateUpdate()
    {
        transform.LookAt((-1) * GameManager.Camera.gameCamera.transform.position);
    }
    
    public void ApplyDamage(int damage)
    {
        _health = Mathf.Max(0, _health - damage);;
        UpdateBar();

        if (_health == 0)
            OnDie?.Invoke();
    }

    public bool IsLastHp(int damage) => _health - damage <= 0;

    public int GetDestroyedHp() => _maxHeath - _health;

    protected internal void SetMaxHealth(int value)
    {
        _maxHeath = value;
        _health = _maxHeath;
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

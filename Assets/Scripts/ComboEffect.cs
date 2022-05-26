using System.Collections;
using TMPro;
using UnityEngine;

public class ComboEffect : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Transform _fire;
    private Timer _timer;
    private const float FirstDecreaseTime = 2.5f;
    private const float DecreaseTime = 0.4f;
    [SerializeField] private float _colorBMultiplier = 3.33f;
    [SerializeField] private float _colorGMultiplier = 1.33f;
    private int _comboCount = 1;
    private int _comboCountFloat;
    private readonly Color _defaultColor = Color.white;
    private Vector3 _basePosition;
    private Vector3 _defaultFireScale;
    private const float HideTime = 0.5f;
    
    private readonly int _fireDecrease = Animator.StringToHash("FireDecrease");
    private readonly int _fireHide = Animator.StringToHash("FireHide");

    private void Awake()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.Init(transform, FirstDecreaseTime, StartDecreaseCombo);
    }

    private void Start()
    {
        _camera = GameManager.Camera.gameCamera;
        _basePosition = transform.position;
        _defaultFireScale = _fire.localScale;

        Hide();
        _comboCount = 2;
        IncreaseCombo();
    }
    
    private void LateUpdate()
    {
        transform.position = _camera.transform.rotation * _basePosition;
        transform.rotation = _camera.transform.rotation;
    }

    public float GetComboCount() => (10 * _comboCount + _comboCountFloat) * 0.1f;
    
    public void IncreaseCombo()
    {
        if (enabled == false)
        {
            Show();
            return;
        }

        _comboCountFloat++;
        if (_comboCountFloat == 10)
        {
            _comboCountFloat = 0;
            _comboCount++;
        }

        UpdateText();
        ResetTimer();
    }

    private void DecreaseCombo()
    {
        _comboCountFloat--;

        if (_comboCountFloat == -1)
        {
            _comboCountFloat = 9;
            _comboCount--;
        }

        if (_comboCount == 0 && _comboCountFloat == 9)
        {
            _comboCountFloat = 0;
            _comboCount = 1;
            _animator.Play(_fireHide);
            StartCoroutine(StartHide());
            return;
        }
        
        _animator.Play(_fireDecrease);
        UpdateText();
    }

    private void StartDecreaseCombo()
    {
        _timer.Action -= StartDecreaseCombo;
        _timer.Action += DecreaseCombo;
        _timer.Time = DecreaseTime;
        _timer.AutoReset = true;
        _timer.ResetTime();
        _timer.Enable();
    }
    
    private void UpdateText()
    {
        var color = _text.color;

        color.g = Mathf.Max(0, 1 - (10 * _comboCount + _comboCountFloat - 11) * 0.1f * _colorGMultiplier);
        color.b = Mathf.Max(0, 1 - (10 * _comboCount + _comboCountFloat - 11) * 0.1f * _colorBMultiplier);
        
        /*if (color.b == 0)
            color.g = Mathf.Max(0, 1 - (10 * _comboCount + _comboCountFloat - (int)(1 / _colorBMultiplier * 0.1f)) * 0.1f * _colorGMultiplier);
        else
            color.b = Mathf.Max(0, 1 - (10 * _comboCount + _comboCountFloat) * 0.1f * _colorBMultiplier);*/

        _text.color = color;
        _text.text = "X." + _comboCount + "." + _comboCountFloat;
    }

    private void ResetTimer()
    {
        if (_timer.Time == DecreaseTime)
        {
            _timer.Action += StartDecreaseCombo;
            _timer.Action -= DecreaseCombo;
            _timer.Time = FirstDecreaseTime;
            _timer.AutoReset = false;
        }
        
        _timer.ResetTime();
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
        enabled = true;
        _timer.Enable();
        UpdateText();
    }

    private void Hide()
    {
        _text.color = _defaultColor;
        _fire.localScale = _defaultFireScale;
        gameObject.SetActive(false);
        enabled = false;
        ResetTimer();
    }

    private IEnumerator StartHide()
    {
        _timer.Disable();
        yield return new WaitForSeconds(HideTime);
        Hide();
    }
}

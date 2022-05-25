using System;
using TMPro;
using UnityEngine;

public class ComboEffect : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private TextMeshProUGUI _text;
    private Timer _timer;
    private const float DecreaseTime = 0.1f;
    private const float ColorDecreaseMultiplier = 0.33f;
    private int _comboCount = 1;
    private int _comboCountFloat;
    private Vector3 _basePosition; 

    private void Awake()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.Init(transform, DecreaseTime, DecreaseCombo, true);
    }

    private void Start()
    {
        _camera = GameManager.Camera.gameCamera;
        _basePosition = transform.position;
    }

    private void FixedUpdate()
    {
        /*var targetPosition = _cameraTarget.rotation * _battleOffset + _cameraTarget.position;
        var relativePos = _enemy.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up) *
                             Quaternion.Euler(new Vector3(_battleRotationOffset, 0, 0));

        if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude < 0.1f &&
            (transform.position - targetPosition).magnitude < 0.1f)
            return;

        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);*/
        
        
        
        transform.position = _camera.transform.rotation * _basePosition;
        transform.rotation = _camera.transform.rotation;// * Quaternion.Euler(0,0,0);
    }

    private void LateUpdate()
    {
        //transform.LookAt((-1) * GameManager.Camera.gameCamera.transform.position);
    }
    
    public void IncreaseCombo()
    {
        if (enabled == false)
            Show();
        
        _comboCountFloat++;
        if (_comboCountFloat == 10)
        {
            _comboCountFloat = 0;
            _comboCount++;
        }

        UpdateText();
    }

    private void DecreaseCombo()
    {
        _comboCountFloat--;
        if (_comboCountFloat == -1)
        {
            _comboCountFloat = 9;
            _comboCount--;
        }

        if (_comboCount == 1 && _comboCountFloat == 0)
        {
            _timer.Disable();
            _timer.ResetTime();
            Hide();
            return;
        }
        
        UpdateText();
    }

    private void UpdateText()
    {
        _text.color = new Color(1, 1 - (10 * _comboCount + _comboCountFloat) * 0.1f * ColorDecreaseMultiplier, 0);
        _text.text = "X." + _comboCount + "." + _comboCountFloat;
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
        _timer.Enable();
        UpdateText();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

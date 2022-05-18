using System;
using UnityEngine;
using UnityEngine.UI;

public class StartView : UIView
{
    [SerializeField] private Button _healthButton;
    [SerializeField] private Button _strengthButton;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _coinsText;

    public Action OnHealthPressed;
    public Action OnStrengthPressed;
    
    private void Awake()
    {
        _healthButton.onClick.AddListener(HealthPressed);
        _strengthButton.onClick.AddListener(StrengthPressed);
    }

    private void HealthPressed() => OnHealthPressed?.Invoke();
    
    private void StrengthPressed() => OnStrengthPressed?.Invoke();
}
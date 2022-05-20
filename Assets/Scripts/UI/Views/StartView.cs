using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : UIView
{
    [SerializeField] private BaseButton _healthButton;
    [SerializeField] private BaseButton _strengthButton;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _coinsText;

    public Action<Store.Upgrades> OnHealthPressed;
    public Action<Store.Upgrades> OnStrengthPressed;
    
    private Dictionary<Store.Upgrades, BaseButton> _buttonsList;
    
    private void Awake()
    {
        _healthButton.Connect(HealthPressed);
        _strengthButton.Connect(StrengthPressed);
        
        _buttonsList = new Dictionary<Store.Upgrades, BaseButton>
        {
            {Store.Upgrades.Health, _healthButton},
            {Store.Upgrades.Strength, _strengthButton}
        };
    }

    private void Start()
    {
        _levelText.text = "LEVEL " + GameManager.GameData.level.ToString();
    }

    public void ToggleUpgrades(List<Store.Upgrades> closedList)
    {
        foreach (var button in _buttonsList)
        {
            button.Value.Toggle(closedList.Contains(button.Key) == false);
        }
    }

    public void UpdateButton(Store.Upgrades type, UpgradeData data)
    {
        _buttonsList[type].UpdateText(data.Level, data.Cost);
    }

    public void UpdateCoins()
    {
        _coinsText.text = GameManager.GameData.coins.ToString();
    }
    
    private void HealthPressed() => OnHealthPressed?.Invoke(Store.Upgrades.Health);
    
    private void StrengthPressed() => OnStrengthPressed?.Invoke(Store.Upgrades.Strength);
}
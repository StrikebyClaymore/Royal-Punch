using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndView : UIView
{
    [SerializeField] private Button _claimButton;
    [SerializeField] private Text _endMessageText;
    [SerializeField] private Text _coinsText;
    [SerializeField] private TextMeshProUGUI _emojiText;

    private const string WinMessage = "SUCCESS";
    private const string LoseMessage = "FAIL";

    public Action OnClaimPressed; 
    
    private void Awake()
    {
        _claimButton.onClick.AddListener(ClaimPressed);
    }

    public override void Show()
    {
        if (GameManager.Win)
        {
            _endMessageText.text = WinMessage;
            _emojiText.enabled = false;
        }
        else
        {
            _endMessageText.text = LoseMessage;
            _emojiText.enabled = true;
        }
        base.Show();
    }
    
    private void ClaimPressed() => OnClaimPressed?.Invoke();
}
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BaseButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] private Image _closedImage;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _costText;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Connect(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

    public void UpdateText(int level, int cost)
    {
        _levelText.text = "LV." + level;
        _costText.text = cost.ToString();
    }
    
    public void Toggle(bool enable)
    {
        _button.interactable = enable;
        _closedImage.enabled = !enable;
    }
}
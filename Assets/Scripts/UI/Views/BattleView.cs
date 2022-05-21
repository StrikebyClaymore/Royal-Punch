using UnityEngine;
using UnityEngine.UI;

public class BattleView : UIView
{
    [SerializeField] private Text _levelText;

    private void Start()
    {
        _levelText.text = "LEVEL " + GameManager.GameData.level;
    }
}
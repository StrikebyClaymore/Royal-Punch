using UnityEngine;
using UnityEngine.EventSystems;

public class StartController : BaseController<StartView>
{
    private void Start()
    {
        ConnectActions();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.StartBattle();
        }
    }

    private void UpgradeHealth()
    {
        
    }

    private void UpgradeStrength()
    {
        
    }
    
    private void ConnectActions()
    {
        ui.OnHealthPressed += UpgradeHealth;
        ui.OnStrengthPressed += UpgradeStrength;
    }
}
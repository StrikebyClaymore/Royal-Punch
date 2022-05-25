using UnityEngine;
using UnityEngine.EventSystems;

public class StartController : BaseController<StartView>
{
    private Store _store;

    private void Awake()
    {
        _store = new Store();
    }

    private void Start()
    {
        ConnectActions();
    }

    public override void Activate()
    {
        base.Activate();
        foreach (var upgrade in _store.GetData())
            ui.UpdateButton(upgrade.Key, upgrade.Value);
        ui.UpdateCoins();
        ui.ToggleUpgrades(_store.CanBuy());
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            GameManager.StartBattle();
        }
#endif
#if UNITY_ANDROID
        foreach (var touch in Input.touches)
        {
            var id = touch.fingerId;
            if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(id) == false)
            {
                GameManager.StartBattle();
            }
        }
#endif
    }

    private void Upgrade(Store.Upgrades type)
    {
        ui.UpdateButton(type, _store.Buy(type));
        ui.UpdateCoins();
        ui.ToggleUpgrades(_store.CanBuy());
    }

    private void ConnectActions()
    {
        ui.OnHealthPressed += Upgrade;
        ui.OnStrengthPressed += Upgrade;
    }
}
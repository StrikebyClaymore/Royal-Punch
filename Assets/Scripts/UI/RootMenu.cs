using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMenu : MonoBehaviour
{
    
    [Header("Controllers")]
    [SerializeField]
    public StartController startController;
    [SerializeField]
    public BattleController battleController;
    [SerializeField]
    public EndController endController;

    public enum ControllerTypes
    {
        Start,
        Battle,
        End
    }

    private void Awake()
    {
        GameManager.RootMenu = this;
        startController.root = this;
        battleController.root = this;
        endController.root = this;
    }

    private void Start()
    {
        ChangeController(ControllerTypes.Start);
    }
    
    public void ChangeController(ControllerTypes controller)
    {
        DeactivateControllers();
        
        switch (controller)
        {
            case ControllerTypes.Start:
                startController.Activate();
                break;
            case ControllerTypes.Battle:
                battleController.Activate();
                break;
            case ControllerTypes.End:
                endController.Activate();
                break;
        }
    }
    
    private void DeactivateControllers()
    {
            startController.Deactivate();
            battleController.Deactivate();
            endController.Deactivate();
    }
}

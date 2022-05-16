using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameCamera Camera = null;
    public static Player Player = null;
    public static Enemy Enemy = null;
    public static PlayerController PlayerController = null;

    public static New.Player Player2 = null;
    public static New.Enemy Enemy2 = null;
    public static New.GameCamera Camera2 = null;

    public static bool BattleIsStarted;
    
    private void Start()
    {
        //PlayerController.LockInput(true);
    }

    public static void StartBattle()
    {
        Camera.StartBattleCamera();
    }
}

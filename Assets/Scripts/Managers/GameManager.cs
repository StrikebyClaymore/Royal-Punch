using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static RootMenu RootMenu;
    public static GameCamera Camera = null;
    public static Player Player = null;
    public static Enemy Enemy = null;
    public static PlayerController PlayerController = null;
    public static GameData GameData;

    public static bool BattleIsStarted;
    public static bool Win;
    private const int WinReward = 50;

    private void Awake()
    {
        GameData = new GameData();
    }

    private void Start()
    {
        PlayerController.LockInput(true);
    }
    
    public static void StartBattle()
    {
        Camera.StartBattleCamera();
    }

    public static void EndBattle()
    {
        GameData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static int AddCoins()
    {
        var enemyHp = Enemy.GetDestroyedHp();
        var reward = enemyHp / 4;
        if (Win)
            reward += WinReward;
        GameData.coins += reward;
        return reward;
    }
    
    public void AddTestCoins()
    {
        GameData.coins += 100;
    }

    public void ResetParams()
    {
        GameData.level = 1;
        GameData.health = 1;
        GameData.strength = 1;
        GameData.coins = 0;
        GameData.Save();
    }
}

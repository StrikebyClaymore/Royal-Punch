using UnityEngine;

public class GameData
{
    public int level = 1;
    public int health = 1;
    public int strength = 1;
    public int coins = 0;

    public GameData()
    {
        Load();
    }
    
    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("health", health);
        PlayerPrefs.SetInt("strength", strength);
        PlayerPrefs.SetInt("coins", coins);
    }
    
    private void Load()
    {
        level = PlayerPrefs.GetInt("level", 1);
        health = PlayerPrefs.GetInt("health", 1);
        strength = PlayerPrefs.GetInt("strength", 1);
        coins = PlayerPrefs.GetInt("coins", 0);
    }
}
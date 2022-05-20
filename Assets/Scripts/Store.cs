using System.Collections.Generic;

public struct UpgradeData
{
    public int Level;
    public int Cost;
    
    public UpgradeData(int level, int cost)
    {
        Level = level;
        Cost = cost;
    }
}

public class Store
{
    private int Health { get => GameManager.GameData.health; set => GameManager.GameData.health = value; }
    private int Strength { get => GameManager.GameData.strength; set => GameManager.GameData.strength = value; }
    private int Coins { get => GameManager.GameData.coins; set => GameManager.GameData.coins = value; }
    
    private const int DefaultCost = 50;
    private const int StartAddCost = 16;
    private const int UpAddCost = 8;

    public enum Upgrades
    {
        Health,
        Strength
    }

    private readonly Dictionary<Upgrades, int> _upgradesList;
    
    public Store()
    {
        _upgradesList = new Dictionary<Upgrades, int>
        {
            {Upgrades.Health, Health},
            {Upgrades.Strength, Strength}
        };
    }
    
    public UpgradeData Buy(Upgrades type)
    {
        Coins -= GetCost(_upgradesList[type]);
        SetValue(type, _upgradesList[type] + 1);
        return new UpgradeData(_upgradesList[type], GetCost(_upgradesList[type]));
    }

    public List<Upgrades> CanBuy()
    {
        var closedList = new List<Upgrades>();
        foreach (var upgrade in _upgradesList)
        {
            if (Coins - GetCost(upgrade.Value) < 0)
            {
                closedList.Add(upgrade.Key);
            }
        }

        return closedList;
    }

    public Dictionary<Upgrades, UpgradeData> GetData()
    {
        var list = new Dictionary<Upgrades, UpgradeData>();
        foreach (var upgrade in _upgradesList)
        {
            list.Add(upgrade.Key, new UpgradeData(upgrade.Value, GetCost(upgrade.Value)));
        }

        return list;
    }
    
    private int GetCost(int level)
    {
        var cost = DefaultCost;
        var addCost = StartAddCost;
        for (int i = 2; i <= level; i++)
        {
            cost += addCost;
            addCost += UpAddCost;
        }

        return cost;
    }

    private int GetValue(Upgrades type)
    {
        int value = 0;
        switch (type)
        {
            case Upgrades.Health:
                value = Health;
                break;
            case Upgrades.Strength:
                value = Strength;
                break;
        }
        return value;
    }

    private void SetValue(Upgrades type, int value)
    {
        switch (type)
        {
            case Upgrades.Health:
                Health = value;
                break;
            case Upgrades.Strength:
                Strength = value;
                break;
        }
        _upgradesList[type] = value;
    }
}
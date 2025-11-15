using System;
using System.Collections.Generic;
using System.Numerics;
public enum CoinType
{
    Copper,
    Silver,
    Gold
}
public class ComponentMoney : IMoneyEngine
{
    public Dictionary<CoinType, int> Money { get; set; }
    public Dictionary<CoinType, int> CoinValues { get; set; }
    public ComponentMoney()
    {
        Dictionary<CoinType, int> Money = new()
        {
            { CoinType.Gold, 0},
            { CoinType.Silver, 0},
            { CoinType.Copper, 0}
        };
        Dictionary<CoinType, int> CoinValues = new()
        {
            { CoinType.Gold, 100 },
            { CoinType.Silver, 10 },
            { CoinType.Copper, 1 }
        };
    }
    public int TotalMoney()
    {
        int totalValue = 0;

        foreach (var coin in Money)
        {
            totalValue += coin.Value * CoinValues[coin.Key];
        }
        return totalValue;
    }
    public void NormalizeCoins()
    {
        int totalMoney = TotalMoney();

        // Clear existing coin counts
        foreach (var key in Money.Keys.ToList())
        {
            Money[key] = 0;
        }

        // Sort coins by value in descending order to use largest denominations first
        var sortedCoins = CoinValues.OrderByDescending(kvp => kvp.Value).ToList();

        foreach (var coin in sortedCoins)
        {
            if (coin.Value <= 0) continue; // Skip invalid coin values to avoid infinite loops
            while (totalMoney >= coin.Value)
            {
                totalMoney -= coin.Value;
                Money[coin.Key] = Money.GetValueOrDefault(coin.Key, 0) + 1;
            }
        }
    }
    public int Trade(PlayerObject player, IMoneyEngine vender)
    {
        int result = 1;
        return result;
    }
}

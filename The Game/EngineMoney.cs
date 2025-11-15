using System;
using System.Numerics;

public interface IMoneyEngine
{
    Dictionary<CoinType, int> Money { get; set; }
    int TotalMoney();
    void NormalizeCoins();
    int Trade(PlayerObject player, IMoneyEngine vender);
}

using System;
using System.Numerics;

namespace The_Game;


public interface IMoneyEngine
{
    Dictionary<CoinType, int> Money { get; set; }
    int TotalMoney();
    void NormalizeCoins();
    int Trade(PlayerObject player, IMoneyEngine vender);
}

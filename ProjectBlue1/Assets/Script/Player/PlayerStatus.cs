using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStatus
{
    public static Status playerStat { get; set; }
    public static float playerHP;
    
    public static void InitFirstStats()
    {
        DataManager.Instance.InitMonsterData();
        DataManager.Instance.InitPlayerData();
        playerStat = DataManager.Instance.playerStats[0];
        playerHP = playerStat.Health;
    }

    public static void LoadStatus()
    {
        playerStat.Attack = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackLv].Attack;
        playerStat.Health = DataManager.Instance.playerStats[SaveDatas.Data.stat.HealthLv].Health;
        playerStat.CriticalHit = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalHitLv].CriticalHit;
        playerStat.CriticalDamage = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalDamageLV].CriticalDamage;
        playerStat.AttackSpeed = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackSpeedLv].AttackSpeed;
    }
}

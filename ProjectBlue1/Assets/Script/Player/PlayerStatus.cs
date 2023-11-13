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
        playerStat = DataManager.Instance.PlayerStats[0];
        playerHP = playerStat.Health;
    }

    public static void LoadStatus()
    {
        playerStat.Attack = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.AttackLv].Attack;
        playerStat.Health = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.HealthLv].Health;
        playerStat.CriticalHit = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.CriticalHitLv].CriticalHit;
        playerStat.CriticalDamage = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.CriticalDamageLV].CriticalDamage;
        playerStat.AttackSpeed = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.AttackSpeedLv].AttackSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Status playerStat { get; set; }
    
    public void InitFirstStats()
    {
        playerStat = DataManager.Instance.playerStats[0];
    }

    public void LoadStatus()
    {
        playerStat.Attack = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackLv].Attack;
        playerStat.Health = DataManager.Instance.playerStats[SaveDatas.Data.stat.HealthLv].Health;
        playerStat.CriticalHit = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalHitLv].CriticalHit;
        playerStat.CriticalDamage = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalDamageLV].CriticalDamage;
        playerStat.AttackSpeed = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackSpeedLv].AttackSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public Status MonStat {  get; set; }

    public float monsterHP;

    public void InitFirstStats()
    {
        MonStat = DataManager.Instance.MonStats[0];
        monsterHP = MonStat.Health;
    }
}

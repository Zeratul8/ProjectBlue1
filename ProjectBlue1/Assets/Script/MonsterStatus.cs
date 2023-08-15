using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public Status MonStat {  get; set; }

    List<Status> MonStats { get; set; }


    private void Awake()
    {
        InitFirstStats();
    }



    public void InitFirstStats()
    {
        ExcelParsing.ParseExcelStatData("MonsterStat", MonStats);
        MonStat = MonStats[Random.Range(0, MonStats.Count)];
    }
}

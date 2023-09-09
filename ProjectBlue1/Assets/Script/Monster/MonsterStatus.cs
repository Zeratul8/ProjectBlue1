using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public Status MonStat {  get; set; }

    List<Status> MonStats { get; set; } = new List<Status>();

    public void InitFirstStats()
    {
        ExcelParsing.ParseExcelStatData("MonsterStat", MonStats);
        MonStat = MonStats[0];
    }
}

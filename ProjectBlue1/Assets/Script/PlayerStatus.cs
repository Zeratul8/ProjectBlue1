using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Status PlayerStat { get; set; }
    List<Status> PlayerStats { get; set; }
    private void Awake()
    {
        InitFirstStats();
    }

    public void InitFirstStats()
    {
        ExcelParsing.ParseExcelStatData("PlayerStat", PlayerStats);
        PlayerStat = PlayerStats[0];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Status PlayerStat { get; set; }
    List<Status> PlayerStats { get; set; } = new List<Status>();
    public void InitFirstStats()
    {
        ExcelParsing.ParseExcelStatData("PlayerStat", PlayerStats);

        PlayerStat = PlayerStats[0];
    }
}

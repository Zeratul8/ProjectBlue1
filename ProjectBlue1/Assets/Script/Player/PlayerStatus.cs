using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Status playerStat { get; set; }
    List<Status> playerStats { get; set; } = new List<Status>();
    public void InitFirstStats()
    {
        ExcelParsing.ParseExcelStatData("PlayerStat", playerStats);

        playerStat = playerStats[0];
    }
}

using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    public List<Status> playerStats { get; set; } = new List<Status>();
    public List<Status> MonStats { get; set; } = new List<Status>();
    public void InitMonsterData()
    {
        ExcelParsing.ParseExcelStatData("MonsterStat", MonStats);
    }
    public void InitPlayerData()
    {
        ExcelParsing.ParseExcelStatData("PlayerStat", playerStats);
    }
}

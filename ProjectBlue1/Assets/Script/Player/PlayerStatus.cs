using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Status PlayerStat { get; set; }
    List<Status> PlayerStats { get; set; } = new List<Status>();
    private void Start()
    {
        InitFirstStats();
    }
    public void InitFirstStats()
    {
        ExcelParsing.ParseExcelStatData("PlayerStat", PlayerStats);
        // 추후 저장된 사용자의 데이터를 가져오게 한다거나 해야할듯..?
        PlayerStat = PlayerStats[0];
    }
    // 추후 스텟 레벨에 따라 가져와야 할 수치를 다르게 해야함
}

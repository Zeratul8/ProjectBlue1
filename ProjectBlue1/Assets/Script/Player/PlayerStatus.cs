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
        // ���� ����� ������� �����͸� �������� �Ѵٰų� �ؾ��ҵ�..?
        PlayerStat = PlayerStats[0];
    }
    // ���� ���� ������ ���� �����;� �� ��ġ�� �ٸ��� �ؾ���
}

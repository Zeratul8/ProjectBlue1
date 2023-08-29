using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStatus stat;

    //public TextMeshProUGUI[] tmp_stats = new TextMeshProUGUI[4]; // 0 = ü��, 1 = ���ݷ�, 2 = ġ�� Ȯ��, 3 = ġ�� ����
    void Awake()
    {
        stat = GetComponent<PlayerStatus>();
        stat.InitFirstStats();
    }

    // Update is called once per frame
    void Update()
    {
        /*tmp_stats[0].text = "���� : " + FormatNumber(stat.playerStat.Health);
        tmp_stats[1].text = "���� : " + FormatNumber(stat.playerStat.Attack);
        tmp_stats[2].text = "���� : " + FormatNumber(stat.playerStat.CriticalHit);
        tmp_stats[3].text = "���� : " + FormatNumber(stat.playerStat.CriticalDamage);*/
    }
    string FormatNumber(float number)
    {
        if(number >= 1000)
        {
            return (number / 1000f).ToString("0.0K");
        }
        else if(number >= 10000)
        {
            return (number / 10000f).ToString("0.0M");
        }
        else
        {
            return number.ToString();
        }
    }
}

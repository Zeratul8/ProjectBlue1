using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStatus stat;

    //public TextMeshProUGUI[] tmp_stats = new TextMeshProUGUI[4]; // 0 = 체력, 1 = 공격력, 2 = 치명 확률, 3 = 치명 피해
    void Awake()
    {
        stat = GetComponent<PlayerStatus>();
        stat.InitFirstStats();
    }

    // Update is called once per frame
    void Update()
    {
        /*tmp_stats[0].text = "현재 : " + FormatNumber(stat.playerStat.Health);
        tmp_stats[1].text = "현재 : " + FormatNumber(stat.playerStat.Attack);
        tmp_stats[2].text = "현재 : " + FormatNumber(stat.playerStat.CriticalHit);
        tmp_stats[3].text = "현재 : " + FormatNumber(stat.playerStat.CriticalDamage);*/
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

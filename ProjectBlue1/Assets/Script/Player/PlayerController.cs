using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStatus stat;

    public TextMeshProUGUI[] tmp_stats = new TextMeshProUGUI[4]; // 0 = 체력, 1 = 공격력, 2 = 치명 확률, 3 = 치명 피해
    void Start()
    {
        stat = GetComponent<PlayerStatus>();
        stat.InitFirstStats();
    }

    // Update is called once per frame
    void Update()
    {
        tmp_stats[0].text = "현재" + stat.PlayerStat.Health;
        tmp_stats[1].text = "현재" + stat.PlayerStat.Attack;
        tmp_stats[2].text = "현재" + stat.PlayerStat.CriticalHit;
        tmp_stats[3].text = "현재" + stat.PlayerStat.CriticalDamage;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStatus stat;

    public TextMeshProUGUI[] tmp_stats = new TextMeshProUGUI[4]; // 0 = ü��, 1 = ���ݷ�, 2 = ġ�� Ȯ��, 3 = ġ�� ����
    void Start()
    {
        stat = GetComponent<PlayerStatus>();
        stat.InitFirstStats();
    }

    // Update is called once per frame
    void Update()
    {
        tmp_stats[0].text = "����" + stat.PlayerStat.Health;
        tmp_stats[1].text = "����" + stat.PlayerStat.Attack;
        tmp_stats[2].text = "����" + stat.PlayerStat.CriticalHit;
        tmp_stats[3].text = "����" + stat.PlayerStat.CriticalDamage;
    }
}

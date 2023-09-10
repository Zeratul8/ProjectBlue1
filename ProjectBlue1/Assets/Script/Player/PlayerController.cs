using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStatus stat;

    public void InitControlPlayer()
    {
        stat = GetComponent<PlayerStatus>();
        stat.InitFirstStats();
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
    public void ResetBattleCondition()
    {
        //���⿡ �÷��̾� �ɾ�� �ִϸ��̼� ������Ѿ���
    }
}

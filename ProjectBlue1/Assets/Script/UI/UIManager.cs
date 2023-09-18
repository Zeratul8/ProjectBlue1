using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.Remoting.Messaging;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public enum StatType
    {
        None = -1,
        Attack,
        Health,
        CriHit,
        CriDmg,
        AttackSpeed,
        Max
    }


    [SerializeField]
    TextMeshProUGUI stageText;
    [SerializeField]
    TextMeshProUGUI goldText;

    [SerializeField]
    UIStatController attackCtr;
    [SerializeField]
    UIStatController healthCtr;
    [SerializeField]
    UIStatController criHitCtr;
    [SerializeField]
    UIStatController criDmgCtr;
    [SerializeField]
    UIStatController attackSpeedCtr;




    protected override void OnStart()
    {
        
    }

    private void Update()
    {
#if UNITY_ANDROID
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGamePopup();
        }
#endif
    }

    public void SetStateText()
    {
        stageText.text = SaveDatas.Data.etc.stage.ToString();
    }
    public void SetGoldText()
    {
        goldText.text = SaveDatas.Data.etc.gold.ToString("F2");
    }

    public void NotEnoughGoldPopup()
    {

    }

    public void ExitGamePopup()
    {
        SaveDatas.Save();
    }

    public void CheckUpgrade(StatType type)
    {
        switch (type)
        {
            case StatType.Attack:
                if (SaveDatas.Data.etc.gold < DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackLv].Cost)
                {
                    NotEnoughGoldPopup();
                    return;
                }
                break;
            case StatType.Health:
                if (SaveDatas.Data.etc.gold < DataManager.Instance.playerStats[SaveDatas.Data.stat.HealthLv].Cost)
                {
                    NotEnoughGoldPopup();
                    return;
                }
                break;
            case StatType.CriHit:
                if (SaveDatas.Data.etc.gold < DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalHitLv].Cost)
                {
                    NotEnoughGoldPopup();
                    return;
                }
                break;
            case StatType.CriDmg:
                if (SaveDatas.Data.etc.gold < DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalDamageLV].Cost)
                {
                    NotEnoughGoldPopup();
                    return;
                }
                break;
            case StatType.AttackSpeed:
                if (SaveDatas.Data.etc.gold < DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackSpeedLv].Cost)
                {
                    NotEnoughGoldPopup();
                    return;
                }
                break;
            default:
                Debug.Log("!!!!!!!UIManager.CheckUpgrade 유효하지 않은 타입값!!!!!!!!");
                break;

        }
        UpgradeStat(type);
    }


    public void UpgradeStat(StatType type)
    {
        // 골드 소모 코드 추가함
        switch(type)
        {
            case StatType.Attack:
                SaveDatas.Data.etc.gold -= DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackLv].Cost;
                SaveDatas.Data.stat.AttackLv++;
                attackCtr.SetText();
                break;
            case StatType.Health:
                SaveDatas.Data.etc.gold -= DataManager.Instance.playerStats[SaveDatas.Data.stat.HealthLv].Cost;
                SaveDatas.Data.stat.HealthLv++;
                healthCtr.SetText();
                break;
            case StatType.CriHit:
                SaveDatas.Data.etc.gold -= DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalHitLv].Cost;
                SaveDatas.Data.stat.CriticalHitLv++;
                criHitCtr.SetText();
                break;
            case StatType.CriDmg:
                SaveDatas.Data.etc.gold -= DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalDamageLV].Cost;
                SaveDatas.Data.stat.CriticalDamageLV++;
                criDmgCtr.SetText();
                break;
            case StatType.AttackSpeed:
                SaveDatas.Data.etc.gold -= DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackSpeedLv].Cost;
                SaveDatas.Data.stat.AttackSpeedLv++;
                attackSpeedCtr.SetText();
                break;
            default:
                Debug.Log("!!!!!!!UIManager.UpgradeStat 유효하지 않은 타입값!!!!!!!!");
                return;

        }
        BattleManager.Instance.SetPlayerStat();
        SetGoldText();
    }
}

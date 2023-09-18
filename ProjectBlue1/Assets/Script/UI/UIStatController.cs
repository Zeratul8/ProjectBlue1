using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIStatController : MonoBehaviour
{
    [SerializeField]
    UIManager.StatType type;
    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    TextMeshProUGUI statText;
    [SerializeField]
    TextMeshProUGUI costText;
    [SerializeField]
    Button upgradeBtn;

    private void Start()
    {
        upgradeBtn.onClick.AddListener(() => UIManager.Instance.CheckUpgrade(type));
        SetText();
    }
    private void OnDestroy()
    {
        upgradeBtn.onClick.RemoveAllListeners();
    }

    public void SetText()
    {
        switch (type)
        {
            case UIManager.StatType.Attack:
                levelText.text = SaveDatas.Data.stat.AttackLv.ToString();
                statText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackLv].Attack.ToString();
                costText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackLv].Cost.ToString("F2");
                break;
            case UIManager.StatType.Health:
                levelText.text = SaveDatas.Data.stat.HealthLv.ToString();
                statText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.HealthLv].Health.ToString();
                costText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.HealthLv].Cost.ToString("F2");
                break;
            case UIManager.StatType.CriHit:
                levelText.text = SaveDatas.Data.stat.CriticalHitLv.ToString();
                statText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalHitLv].CriticalHit.ToString() + " %";
                costText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalHitLv].Cost.ToString("F2");
                break;
            case UIManager.StatType.CriDmg:
                levelText.text = SaveDatas.Data.stat.CriticalDamageLV.ToString();
                statText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalDamageLV].CriticalDamage.ToString();
                costText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalDamageLV].Cost.ToString("F2");
                break;
            case UIManager.StatType.AttackSpeed:
                levelText.text = SaveDatas.Data.stat.AttackSpeedLv.ToString();
                statText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackSpeedLv].AttackSpeed.ToString();
                costText.text = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackSpeedLv].Cost.ToString("F2");
                break;

        }
    }
}

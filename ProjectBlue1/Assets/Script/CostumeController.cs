using HeroEditor.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostumeController : MonoBehaviour
{

    public int price;
    public CostumeType type;
    public bool possession;

    [SerializeField]
    Image costumeImg;
    [SerializeField]
    private TextMeshProUGUI costumePrice;
    [SerializeField]
    private Button costumeBtn;
    [SerializeField]
    private Button lockBtn;

    public Image CostumeImg { get { return costumeImg; } }

    private void Start()
    {
        costumePrice.text = Constants.weaponPrice.ToString();
        
        if(possession)
        {
            lockBtn.gameObject.SetActive(false);
        }

        costumeBtn.onClick.AddListener(CostumeChange);
        lockBtn.onClick.AddListener(CostumeBuy);
    }
    void CostumeBuy()
    {
        if (CostumeManager.Instance.CostumeBuy(Constants.weaponPrice))
        {
            possession = true;
            lockBtn.gameObject.SetActive(false);
            SaveDatas.Data.costume.costumes.Add(costumeImg.sprite.name);
            SaveDatas.SaveServer();
        }
    }
    void CostumeChange()
    {
        CostumeManager.Instance.SwordCostumeChange(costumeImg.sprite, possession);
    }

    private void OnDestroy()
    {
        costumeBtn.onClick.RemoveAllListeners();
        lockBtn.onClick.RemoveAllListeners();
    }

}

using HeroEditor.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostumeController : MonoBehaviour
{ 
    public enum Type
    {
        None,
        Sword,
        Max
    }
    public string cosName;
    public Sprite sprite;
    public int price;
    public Type type;
    public bool possession;

    [SerializeField]
    private Image costumeImg;
    [SerializeField]
    private TextMeshProUGUI costumePrice;
    [SerializeField]
    private Button costumeBtn;
    [SerializeField]
    private Button lockBtn;

    private void Start()
    {
        costumeImg.sprite = sprite;
        costumePrice.text = price.ToString();

        costumeBtn.onClick.AddListener(() => CostumeChange());
        lockBtn.onClick.AddListener(() => CostumeBuy());
    }
    void CostumeBuy()
    {
        if (CostumeManager.Instance.CostumeBuy(price))
        {
            possession = true;
            lockBtn.gameObject.SetActive(false);
        }
    }
    void CostumeChange()
    {
        CostumeManager.Instance.SwordCostumeChange(sprite, possession);
    }
    
}

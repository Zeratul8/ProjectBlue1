using HeroEditor.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CostumeManager : SingletonMonoBehaviour<CostumeManager>
{

    public CharacterBase playerCharacterBase;


    [SerializeField]
    CostumeIconCreater creater;

    List<CostumeController> costumeControllers = new List<CostumeController>();


    private IEnumerator Start()
    {
        while (true)
        {
            if (creater.enabled)
                break;
            yield return null;
        }
        costumeControllers = creater.CreateIcons();
        CheckCostumesHave();
        foreach (var controller in costumeControllers)
        {
            controller.gameObject.SetActive(true);
        }
    }





    public void SwordCostumeChange(Sprite costumeSprite, bool costumePossession)
    {
        if(costumePossession)
        {
            playerCharacterBase.PrimaryMeleeWeaponRenderer.sprite = costumeSprite;
        }
    }

    public bool CostumeBuy(int costumePrice)
    {
        if(SaveDatas.Data.etc.cristal >= costumePrice)
        {
            PopUp.Instance.SetText(PopUp.PopUpType.oneBtn, "알림", "구매 성공 !");
            SaveDatas.Data.etc.cristal -= costumePrice;
            return true;
        }
        PopUp.Instance.SetText(PopUp.PopUpType.oneBtn, "알림", "구매 실패 !");
        return false;
    }

    public void CheckCostumesHave()
    {
        if (SaveDatas.Data?.costume == null) return;
        if (SaveDatas.Data.costume.costumes?.Count < 1) return;
        Debug.Log("체크시작!!");
        List<string> spriteNames = SaveDatas.Data.costume.costumes;
        foreach (string spriteName in spriteNames)
        {
            foreach(CostumeController costumeController in costumeControllers)
            {
                if (spriteName.Equals(costumeController.CostumeImg.sprite.name))
                {
                    costumeController.possession = true;
                }    
            }
        }
    }

}

using HeroEditor.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CostumeManager : SingletonMonoBehaviour<CostumeManager>
{

    public CharacterBase playerCharacterBase;

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
            PopUp.Instance.SetText(PopUp.PopUpType.oneBtn, "�˸�", "���� ���� !");
            SaveDatas.Data.etc.cristal -= costumePrice;
            return true;
        }
        PopUp.Instance.SetText(PopUp.PopUpType.oneBtn, "�˸�", "���� ���� !");
        return false;
    }
}

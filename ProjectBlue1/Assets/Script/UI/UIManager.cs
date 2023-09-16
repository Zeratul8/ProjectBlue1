using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField]
    TextMeshProUGUI stageText;
    [SerializeField]
    TextMeshProUGUI goldText;




    protected override void OnStart()
    {
        
    }

    public void SetStateText()
    {
        stageText.text = SaveDatas.Data.etc.stage.ToString();
    }
    public void SetGoldText()
    {
        goldText.text = SaveDatas.Data.etc.gold.ToString();
    }
}

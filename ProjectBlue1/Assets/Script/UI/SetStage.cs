using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetStage : MonoBehaviour
{
    TextMeshProUGUI stageText;

    int stageNumber;


    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI[] tmp = GetComponentsInChildren<TextMeshProUGUI>();
        stageText = tmp[1];

        //저장된 스테이지 정보로 불러와야함, 임시숫자
        stageNumber = 1;
    }

    public void NextStageNumber()
    {
        stageNumber++;
    }
}

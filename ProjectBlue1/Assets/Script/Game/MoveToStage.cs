using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MoveToStage : MonoBehaviour
{
    [SerializeField]
    Button moveStageBtn;

    [SerializeField]
    GameObject stagePopup;

    [SerializeField]
    Button okBtn;

    [SerializeField]
    TMP_InputField stageInput;

    // Start is called before the first frame update
    void Start()
    {
        moveStageBtn.onClick.AddListener(OpenStagePopup);
        okBtn.onClick.AddListener(GoToStage);
    }

    void OpenStagePopup()
    {
        stagePopup.SetActive(true);
    }
    
    void GoToStage()
    {
        int stageNum;
        if (!int.TryParse(stageInput.text, out stageNum)) return;
        if (SaveDatas.Data.etc.highestClearedStage < stageNum) return;

        SaveDatas.Data.etc.stage = stageNum;
        SceneManager.LoadSceneAsync("GamePlay");
    }

    public void ClosePopup()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : SingletonMonoBehaviour<PopUp>
{
    public enum PopUpType
    {
        None,
        oneBtn,
        twoBtn,
        Max
    }
    private bool isPopup;
    private PopUpType selectedType;

    [SerializeField]
    public Button panelBtn;

    private GameObject oneObj;
    private GameObject twoObj;

    private TextMeshProUGUI titleTMP;
    private TextMeshProUGUI contentTMP;

    private void Start()
    {
        panelBtn.gameObject.SetActive(false);
        oneObj = panelBtn.gameObject.transform.Find("IMG_OneBtn").gameObject;
        twoObj = panelBtn.gameObject.transform.Find("IMG_TwoBtn").gameObject;
        oneObj.SetActive(false);
        twoObj.SetActive(false);
        panelBtn.onClick.AddListener(()=> { panelBtn.gameObject.SetActive(false); PopUpSwitch(selectedType); });
    }
    private void OnDestroy()
    {
        panelBtn.onClick.RemoveAllListeners();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetText(PopUpType.oneBtn, "버튼 하나용", "확인 되셨습니다");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetText(PopUpType.twoBtn, "버튼 두개용", "확인 하셨습니까 ?");
        }
    }
    private void PopUpSwitch(PopUpType type)
    {
        isPopup = !isPopup;
        panelBtn.gameObject.SetActive(isPopup);


        if (type == PopUpType.oneBtn)
        {
            oneObj.SetActive(isPopup);
        }
        else if (type == PopUpType.twoBtn)
        {
            twoObj.SetActive(isPopup);
        }


    }
    // 해당 타입에 대한 오브젝트의 자식 TMP들을 가져옴
    private void InitComponent(PopUpType type)
    {
        Debug.Log("Init 실행");
        if(type == PopUpType.oneBtn)
        {
            titleTMP = oneObj.transform.Find("TMP_Title").GetComponent<TextMeshProUGUI>();
            contentTMP = oneObj.transform.Find("TMP_Content").GetComponent<TextMeshProUGUI>();
        }
        else if(type == PopUpType.twoBtn)
        {
            titleTMP = twoObj.transform.Find("TMP_Title").GetComponent<TextMeshProUGUI>();
            contentTMP = twoObj.transform.Find("TMP_Content").GetComponent<TextMeshProUGUI>();
        }
    }

    // 해당 타입에 대한 텍스트들을 수정함
    public void SetText(PopUpType type, string title, string content)
    {
        selectedType = type;
        PopUpSwitch(type);
        InitComponent(type);
        titleTMP.text = title;
        contentTMP.text = content;
    }
}

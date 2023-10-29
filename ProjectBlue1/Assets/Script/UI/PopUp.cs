using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    // twoObj에서 '네'가 눌렸다면 true
    public bool isAccept;

    private PopUpType selectedType;

    [SerializeField]
    public Button panelBtn;

    private Button oneBtn;

    private Button twoAcceptBtn;
    private Button twoCancelBtn;

    private GameObject oneObj;
    private GameObject twoObj;

    private TextMeshProUGUI titleTMP;
    private TextMeshProUGUI contentTMP;

    private void Start()
    {
        panelBtn.gameObject.SetActive(true);

        var images = GetComponentsInChildren<Image>(true);
        foreach(var image in images)
        {
            if(image.gameObject.name.Equals("IMG_OneBtn"))
                oneObj = image.gameObject;
            if(image.gameObject.name.Equals("IMG_TwoBtn"))
                twoObj = image.gameObject;
        }
        var oneButtons = oneObj.GetComponentsInChildren<Button>(true);
        foreach(var button in oneButtons)
        {
            if(button.gameObject.name.Equals("BTN_Yes"))
            {
                oneBtn = button;
                break;
            }
        }
        //oneObj = panelBtn.gameObject.transform.Find("IMG_OneBtn").gameObject;
        //twoObj = panelBtn.gameObject.transform.Find("IMG_TwoBtn").gameObject;
        var twoButtons = twoObj.GetComponentsInChildren<Button>(true);
        foreach(var button in twoButtons)
        {
            if(button.gameObject.name.Equals("BTN_Yes"))
            {
                twoAcceptBtn = button;
            }
            if(button.gameObject.name.Equals("BTN_No"))
            {
                twoCancelBtn = button;
            }
        }
        //acceptBtn = twoObj.transform.Find("BTN_Yes").GetComponent<Button>();

        // 패널 false, 현재 열린 팝업 false
        panelBtn.onClick.AddListener(()=> ClosePopUp());
        // '네' 를 클릭하면 isAccept = true
        twoAcceptBtn.onClick.AddListener(() => { ClosePopUp(); isAccept = true; });
        twoCancelBtn.onClick.AddListener(() => { ClosePopUp(); });
        // '확인', '아니오'를 누르면 버튼 컴포넌트에 있는 OnClick 이벤트로 ClosePopUp 실행


        oneObj.SetActive(false);
        twoObj.SetActive(false);
        panelBtn.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        panelBtn.onClick.RemoveAllListeners();
    }
    // Update는 테스트용
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetText(PopUpType.oneBtn, "버튼 하나용", "확인 되셨습니다");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetText(PopUpType.twoBtn, "버튼 두개용", "확인 하셨습니까 ?");
        }
    }*/
    public void OpenPopUp(PopUpType type)
    {
        //isPopup = !isPopup;
        oneObj.SetActive(false);
        twoObj.SetActive(false);
        panelBtn.gameObject.SetActive(true);


        if (type == PopUpType.oneBtn)
        {
            oneObj.SetActive(true);
        }
        else if (type == PopUpType.twoBtn)
        {
            twoObj.SetActive(true);
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
    // 현재 열린 팝업을 닫음
    public void ClosePopUp()
    {
        isAccept = false;
        panelBtn.gameObject.SetActive(false);
        //OpenPopUp(selectedType);
    }

    // 해당 타입에 대한 텍스트들을 수정함
    public void SetText(PopUpType type, string title, string content)
    {
        selectedType = type;
        OpenPopUp(type);
        InitComponent(type);
        titleTMP.text = title;
        contentTMP.text = content;
    }

    public void SetOneButtonPopup(UnityAction action)
    {
        oneBtn.onClick.RemoveAllListeners();
        oneBtn.onClick.AddListener(action + ClosePopUp);
        panelBtn.onClick.RemoveAllListeners();
        panelBtn.onClick.AddListener(action + ClosePopUp);
    }

    public void SetTwoButtonPopup(UnityAction okAction, UnityAction cancelAction)
    {
        twoAcceptBtn.onClick.RemoveAllListeners();
        twoAcceptBtn.onClick.AddListener(okAction + ClosePopUp);
        twoCancelBtn.onClick.RemoveAllListeners();
        twoAcceptBtn.onClick.AddListener(cancelAction + ClosePopUp);
        panelBtn.onClick.RemoveAllListeners();
        panelBtn.onClick.AddListener(cancelAction + ClosePopUp);
    }
}

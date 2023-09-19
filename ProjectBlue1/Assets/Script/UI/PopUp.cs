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
    // twoObj에서 '네'가 눌렸다면 true
    public bool isAccept;

    private PopUpType selectedType;

    [SerializeField]
    public Button panelBtn;
    private Button acceptBtn;

    private GameObject oneObj;
    private GameObject twoObj;

    private TextMeshProUGUI titleTMP;
    private TextMeshProUGUI contentTMP;

    private void Start()
    {
        panelBtn.gameObject.SetActive(false);
        oneObj = panelBtn.gameObject.transform.Find("IMG_OneBtn").gameObject;
        twoObj = panelBtn.gameObject.transform.Find("IMG_TwoBtn").gameObject;
        acceptBtn = twoObj.transform.Find("BTN_Yes").GetComponent<Button>();
        oneObj.SetActive(false);
        twoObj.SetActive(false);

        // 패널 false, 현재 열린 팝업 false
        panelBtn.onClick.AddListener(()=> ClosePopUp());
        // '네' 를 클릭하면 isAccept = true
        acceptBtn.onClick.AddListener(() => { ClosePopUp(); isAccept = true; });
        // '확인', '아니오'를 누르면 버튼 컴포넌트에 있는 OnClick 이벤트로 ClosePopUp 실행
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
    private void OpenPopUp(PopUpType type)
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
    // 현재 열린 팝업을 닫음
    public void ClosePopUp()
    {
        isAccept = false;
        panelBtn.gameObject.SetActive(false);
        OpenPopUp(selectedType);
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
}

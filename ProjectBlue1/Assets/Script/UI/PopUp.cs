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
    // twoObj���� '��'�� ���ȴٸ� true
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

        // �г� false, ���� ���� �˾� false
        panelBtn.onClick.AddListener(()=> ClosePopUp());
        // '��' �� Ŭ���ϸ� isAccept = true
        acceptBtn.onClick.AddListener(() => { ClosePopUp(); isAccept = true; });
        // 'Ȯ��', '�ƴϿ�'�� ������ ��ư ������Ʈ�� �ִ� OnClick �̺�Ʈ�� ClosePopUp ����
    }
    private void OnDestroy()
    {
        panelBtn.onClick.RemoveAllListeners();
    }
    // Update�� �׽�Ʈ��
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetText(PopUpType.oneBtn, "��ư �ϳ���", "Ȯ�� �Ǽ̽��ϴ�");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetText(PopUpType.twoBtn, "��ư �ΰ���", "Ȯ�� �ϼ̽��ϱ� ?");
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
    // �ش� Ÿ�Կ� ���� ������Ʈ�� �ڽ� TMP���� ������
    private void InitComponent(PopUpType type)
    {
        Debug.Log("Init ����");
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
    // ���� ���� �˾��� ����
    public void ClosePopUp()
    {
        isAccept = false;
        panelBtn.gameObject.SetActive(false);
        OpenPopUp(selectedType);
    }

    // �ش� Ÿ�Կ� ���� �ؽ�Ʈ���� ������
    public void SetText(PopUpType type, string title, string content)
    {
        selectedType = type;
        OpenPopUp(type);
        InitComponent(type);
        titleTMP.text = title;
        contentTMP.text = content;
    }
}

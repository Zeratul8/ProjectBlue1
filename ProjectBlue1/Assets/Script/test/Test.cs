using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField]
    GoogleLogin google;
    [SerializeField]
    Button login;
    [SerializeField]
    Button save;
    [SerializeField]
    Button load;
    [SerializeField]
    Button delete;
    [SerializeField]
    TextMeshProUGUI log;

    private GameData data = new GameData();
    // Start is called before the first frame update
    void Start()
    {
        login.onClick.AddListener(()=> { google.LoginGPGS(); log.text = "로그인!"; });
        save.onClick.AddListener(() => { GPGSSaveData.Instance.SaveData(JsonUtility.ToJson(data)); log.text = "저장완료"; });
        load.onClick.AddListener(()=> { GPGSSaveData.Instance.LoadData(); log.text = SaveDatas.Data.etc.gold.ToString(); });
        delete.onClick.AddListener(()=> { GPGSSaveData.Instance.DeleteData(); log.text = "삭제완료"; });
    }
}

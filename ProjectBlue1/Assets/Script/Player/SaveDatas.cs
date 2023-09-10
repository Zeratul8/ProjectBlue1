using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveDatas : MonoBehaviour
{
    string savePath;

    public PlayerStatus stat;
    GameData Data;
    private void Start()
    {
        Data = new GameData();
        stat = GetComponent<PlayerStatus>();
        savePath = Application.persistentDataPath + "/userSaveData.json";
        Load();
    }
    public void ReceiveData(int currentStage = 1, float currentGold = 0, int currentDiamond = 0)
    {
        Data.etc.stage = currentStage;
        Data.etc.gold= currentGold;
        Data.etc.diamond= currentDiamond;
    }
    [ContextMenu("Save")]
    private void Save()
    {
        // JSON 문자열로 변환
        string jsonStr = Data.JsonFormat();
        // 세이브 경로로 JSON 파일 저장
        File.WriteAllText(savePath, jsonStr);
        Debug.Log("데이터 저장에 성공하였습니다!!!!");
    }
    [ContextMenu("Load")]
    private void Load()
    {
        if(!File.Exists(savePath))
        {
            stat.InitFirstStats();
            Data.stat = stat.playerStat;
            ReceiveData();
            Save();
            Debug.Log("세이브 파일이 없어, 새로 만들고 초기값을 설정했습니다!!!!");
            return;
        }
 
        string jsonStr = File.ReadAllText(savePath);
        Data = JsonUtility.FromJson<GameData>(jsonStr);
        Debug.Log("데이터 로드 성공! 아래는 불러온 스텟");
        Debug.Log("체력: " + Data.stat.Health);
        Debug.Log("공격력: " + Data.stat.Attack);
        Debug.Log("치명확률: " + Data.stat.CriticalHit);
        Debug.Log("치명피해: " + Data.stat.CriticalDamage);
        Debug.Log("스테이지: " + Data.etc.stage);
        Debug.Log("골드: " + Data.etc.gold);
        Debug.Log("다이아: " + Data.etc.diamond);
    }
}
class GameData
{
    public Status stat = new Status();
    public EtcData etc = new EtcData();

    public string JsonFormat()
    {
        string statJson = JsonUtility.ToJson(stat, true);
        string etcJson = JsonUtility.ToJson(etc, true);

        string jsonStr = "{\"stat\":\n" + statJson + ",\n\"etc\":" + etcJson + "\n}";

        return jsonStr;
    }
}
class EtcData
{
    public int stage;
    public float gold;
    public int diamond;
    // 추가될 정보들은 아래로 적기
}
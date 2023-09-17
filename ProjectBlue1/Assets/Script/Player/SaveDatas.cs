using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using System;
using JetBrains.Annotations;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SaveDatas
{
    static string savePath = Application.persistentDataPath + "/userSaveData.json";

    public static PlayerStatus stat;
    public static GameData Data = new GameData();
    /*private void Start()
    {
        Data = new GameData();
        stat = GetComponent<PlayerStatus>();
        savePath = Application.persistentDataPath + "/userSaveData.json";
        Load();
    }*/
    public static void ReceiveData(int currentStage = 1, float currentGold = 0, int currentDiamond = 0)
    {
        Data.etc.stage = currentStage;
        Data.etc.gold= currentGold;
        Data.etc.diamond= currentDiamond;
    }
    //서버 수동,자동세이브 넣을예정
    //[ContextMenu("Save")]
    [MenuItem("Test/Save")]
    public static void Save()
    {
        stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        //게임데이터 여기다 불러와야함!

        // JSON 문자열로 변환
        string jsonStr = JsonUtility.ToJson(Data);
        // 세이브 경로로 JSON 파일 저장
        File.WriteAllText(savePath, jsonStr);
        Debug.Log("데이터 저장에 성공하였습니다!!!!");
    }
    //[ContextMenu("Load")]
    [MenuItem("Test/Load")]
    public static void Load()
    {
        stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        if(!File.Exists(savePath))
        {
            stat.InitFirstStats();
            ReceiveData();
            Save();
            Debug.Log("세이브 파일이 없어, 새로 만들고 초기값을 설정했습니다!!!!");
            return;
        }
 
        string jsonStr = File.ReadAllText(savePath);
        Data = JsonUtility.FromJson<GameData>(jsonStr);
        Debug.Log("데이터 로드 성공! 아래는 불러온 스텟");
        Debug.Log("체력: " + Data.stat.HealthLv);
        Debug.Log("공격력: " + Data.stat.AttackLv);
        Debug.Log("치명확률: " + Data.stat.CriticalHitLv);
        Debug.Log("치명피해: " + Data.stat.CriticalDamageLV);
        Debug.Log("스테이지: " + Data.etc.stage);
        Debug.Log("골드: " + Data.etc.gold);
        Debug.Log("다이아: " + Data.etc.diamond);

    }
}
[Serializable]
public class GameData
{
    public StatLvData stat = new StatLvData();
    public EtcData etc = new EtcData();
}
[Serializable]
public class StatLvData
{
    public int AttackLv;
    public int HealthLv;
    public int CriticalHitLv;
    public int CriticalDamageLV;
    public int AttackSpeedLv;
}


[Serializable]
public class EtcData
{
    public int stage;
    public float gold;
    public int diamond;
    // 추가될 정보들은 아래로 적기
}
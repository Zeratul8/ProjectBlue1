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
    public static GameData Data = new GameData();
    /*private void Start()
    {
        Data = new GameData();
        stat = GetComponent<PlayerStatus>();
        savePath = Application.persistentDataPath + "/userSaveData.json";
        Load();
    }*/
    public static void ReceiveData(int currentStage = 1, float currentGold = 0, int currentCristal = 0)
    {
        Data.etc.stage = currentStage;
        Data.etc.gold= currentGold;
        Data.etc.cristal= currentCristal;
    }
    //���� ����,�ڵ����̺� ��������
    //[ContextMenu("Save")]
#if UNITY_EDITOR
    [MenuItem("Test/Save")]
#endif
    public static void Save()
    {
        //���ӵ����� ����� �ҷ��;���!

        // JSON ���ڿ��� ��ȯ
        string jsonStr = JsonUtility.ToJson(Data);
        // ���̺� ��η� JSON ���� ����
        GPGSSaveData.Instance.SaveData(jsonStr);
        File.WriteAllText(savePath, jsonStr);
        Debug.Log("������ ���忡 �����Ͽ����ϴ�!!!!");
    }
    //[ContextMenu("Load")]
    public static void Load(string jsonData)
    {
        Data = JsonUtility.FromJson<GameData>(jsonData);
        Debug.Log("������ �ε� ����! �Ʒ��� �ҷ��� ����");
        Debug.Log("ü��: " + Data.stat.HealthLv);
        Debug.Log("���ݷ�: " + Data.stat.AttackLv);
        Debug.Log("ġ��Ȯ��: " + Data.stat.CriticalHitLv);
        Debug.Log("ġ������: " + Data.stat.CriticalDamageLV);
        Debug.Log("��������: " + Data.etc.stage);
        Debug.Log("���: " + Data.etc.gold);
        Debug.Log("���̾�: " + Data.etc.cristal);

    }
    public static void LoadFail()
    {
        if (!File.Exists(savePath))
        {
            PlayerStatus.InitFirstStats();
            ReceiveData();
            Save();
            Debug.Log("���̺� ������ ����, ���� ����� �ʱⰪ�� �����߽��ϴ�!!!!");
            return;
        }
        string jsonStr = File.ReadAllText(savePath);
        Data = JsonUtility.FromJson<GameData>(jsonStr);
    }


    public static void SetHighestStage()
    {
        if(Data.etc.stage > Data.etc.highestClearedStage)
            Data.etc.highestClearedStage = Data.etc.stage;
    }
}
[Serializable]
public class GameData
{
    public StatLvData stat = new StatLvData();
    public EtcData etc = new EtcData();
    public CostumeData costume = new CostumeData();
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
    public int highestClearedStage;
    public float gold;
    public int cristal;
    // �߰��� �������� �Ʒ��� ����
}

[Serializable]
public class CostumeData
{
    public List<string> costumes = new List<string>();
}
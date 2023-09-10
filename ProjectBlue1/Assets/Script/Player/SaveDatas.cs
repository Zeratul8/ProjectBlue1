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
        // JSON ���ڿ��� ��ȯ
        string jsonStr = Data.JsonFormat();
        // ���̺� ��η� JSON ���� ����
        File.WriteAllText(savePath, jsonStr);
        Debug.Log("������ ���忡 �����Ͽ����ϴ�!!!!");
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
            Debug.Log("���̺� ������ ����, ���� ����� �ʱⰪ�� �����߽��ϴ�!!!!");
            return;
        }
 
        string jsonStr = File.ReadAllText(savePath);
        Data = JsonUtility.FromJson<GameData>(jsonStr);
        Debug.Log("������ �ε� ����! �Ʒ��� �ҷ��� ����");
        Debug.Log("ü��: " + Data.stat.Health);
        Debug.Log("���ݷ�: " + Data.stat.Attack);
        Debug.Log("ġ��Ȯ��: " + Data.stat.CriticalHit);
        Debug.Log("ġ������: " + Data.stat.CriticalDamage);
        Debug.Log("��������: " + Data.etc.stage);
        Debug.Log("���: " + Data.etc.gold);
        Debug.Log("���̾�: " + Data.etc.diamond);
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
    // �߰��� �������� �Ʒ��� ����
}
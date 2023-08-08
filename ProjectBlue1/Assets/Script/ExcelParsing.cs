using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelParsing : MonoBehaviour
{
    [Header("JSON File Input")]
    public TextAsset file;
    private AllStatus datas;
    private void Awake()
    {
        datas = JsonUtility.FromJson<AllStatus>(file.text);
        Debug.Log(datas.status[1].Health);
    }
}

[System.Serializable]
public class AllStatus
{
    public Status[] status;
}

[System.Serializable]
public class Status
{
    public int Attack;
    public int Health;
    public int CriticalHit;
    public int CriticalDamage;
}

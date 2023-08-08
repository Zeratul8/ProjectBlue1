using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelParsing : MonoBehaviour
{
    public TextAsset file;
    private AllStatus datas;
    private void Awake()
    {
        datas = JsonUtility.FromJson<AllStatus>(file.ToString());

        foreach (var value in datas.status)
        {
            print(value.Attack);
        }
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
    public float Attack;
    public float Health;
    public float CriticalHit;
    public float CriticalDamage;
}

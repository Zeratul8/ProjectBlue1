using Assets.HeroEditor.Common.Scripts.ExampleScripts;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataManager : SingletonMonoBehaviour<DataManager>
{

    readonly string playerDataName = "PlayerStats";
    readonly string monsterDataName = "Assets/Excel/MonsterStats.csv";
    //readonly string test = "Assets/BalanceEditor/Data/BalanceData.csv";

    List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();


    public List<Status> PlayerStats { get; set; } = new List<Status>();
    public List<Status> MonStats { get; set; } = new List<Status>();

    protected override void OnAwake()
    {
        LoadPlayerData();
        LoadMonsterData();
    }
    
    
    public void LoadPlayerData()
    {
        StartCoroutine(Addressable_LoadPlayerData());
    }

    IEnumerator Addressable_LoadPlayerData()
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(playerDataName);



        //var handle = Addressables.InstantiateAsync(assetReference);

        handles.Add(handle);
        yield return handle;

        var readData = BlueCSVReader.Read(handle.Result);

        int readCount = readData.Count;
        for(int i =0; i < readCount; i++)
        {
            Status status = new Status();
            status.Attack = readData[i]["Attack"];
            status.Health = readData[i]["Health"];
            status.CriticalHit = readData[i]["CriticalHit"];
            status.CriticalDamage = readData[i]["CriticalDamage"];
            status.AttackSpeed = readData[i]["AttackSpeed"];
            status.Cost = readData[i]["Cost"];

            PlayerStats.Add(status);
        }

        InitPlayerData();
    }

    public void LoadMonsterData()
    {
        StartCoroutine(Addressable_LoadMonsterData());
    }

    IEnumerator Addressable_LoadMonsterData()
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(monsterDataName);



        //var handle = Addressables.InstantiateAsync(assetReference);

        handles.Add(handle);
        yield return handle;

        var readData = BlueCSVReader.Read(handle.Result);

        int readCount = readData.Count;
        for (int i = 0; i < readCount; i++)
        {
            Status status = new Status();
            status.Attack = readData[i]["Attack"];
            status.Health = readData[i]["Health"];
            status.CriticalHit = readData[i]["CriticalHit"];
            status.CriticalDamage = readData[i]["CriticalDamage"];
            status.AttackSpeed = readData[i]["AttackSpeed"];
            status.Cost = readData[i]["Cost"];

            MonStats.Add(status);
        }

        InitPlayerData();
    }


    public void InitMonsterData()
    {
        //ExcelParsing.ParseExcelStatData(monsterDataStream, MonStats);
    }
    public void InitPlayerData()
    {
        //ExcelParsing.ParseExcelStatData(playerDataStream, playerStats);
    }
    void OnDestroy()
    {
        Addressables.Release(handles);
    }
}

[Serializable]
public class Status
{
    public float Attack;
    public float Health;
    public float CriticalHit;
    public float CriticalDamage;
    public float AttackSpeed;
    public float Cost;
}

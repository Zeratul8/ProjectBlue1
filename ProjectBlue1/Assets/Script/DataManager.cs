using GooglePlayGames.BasicApi;
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
    Stream playerDataStream;
    Stream monsterDataStream;

    List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();


    public List<Status> playerStats { get; set; } = new List<Status>();
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

        playerDataStream = new MemoryStream(handle.Result.bytes);
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

        playerDataStream = new MemoryStream(handle.Result.bytes);
        InitPlayerData();
    }


    public void InitMonsterData()
    {
        ExcelParsing.ParseExcelStatData(monsterDataStream, MonStats);
    }
    public void InitPlayerData()
    {
        ExcelParsing.ParseExcelStatData(playerDataStream, playerStats);
    }
    void OnDestroy()
    {
        Addressables.Release(handles);
    }
}

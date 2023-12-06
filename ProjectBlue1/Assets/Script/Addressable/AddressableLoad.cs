using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoad : SingletonMonoBehaviour<AddressableLoad>
{
    [SerializeField]
    AssetReference assetReference;

    readonly string playerDataName = "PlayerStats";
    readonly string monsterDataName = "Assets/Excel/MonsterStats.csv";

    List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();


    public void LoadPlayerData()
    {
        StartCoroutine(Addressable_LoadPlayerData());
    }

    IEnumerator Addressable_LoadPlayerData()
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(playerDataName);


        handles.Add(handle);
        yield return handle;

        Stream stream = new MemoryStream(handle.Result.bytes);
        
        
        Debug.Log("°á°ú°ª!!!!" + handle.Result.ToString());
    }

    void OnDestroy()
    {
        Addressables.Release(handles);
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

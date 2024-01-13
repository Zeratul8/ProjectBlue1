using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GPGSSaveData : SingletonMonoBehaviour<GPGSSaveData>
{
    bool isReady;
    public bool IsReady { get { return isReady; } }
    protected override void OnStart()
    {
        //StartCoroutine(Coroutine_AutoSave());
    }

    public ISavedGameClient SavedGame()
    {
        return PlayGamesPlatform.Instance.SavedGame;
    }

    public void SaveData(string jsonData)
    {
        SavedGame().OpenWithAutomaticConflictResolution("UserSaveData", DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (status, data) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    var update = new SavedGameMetadataUpdate.Builder().Build();
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
                    SavedGame().CommitUpdate(data,update,bytes, SaveCloudData);
                }    
            });
    }

    public void SaveDataAndQuit(string jsonData)
    {
        SavedGame().OpenWithAutomaticConflictResolution("UserSaveData", DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (status, data) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    var update = new SavedGameMetadataUpdate.Builder().Build();
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
                    SavedGame().CommitUpdate(data, update, bytes, SaveCloudData);
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_ANDROID
        Application.Quit();
#endif
                }
                else
                {
                    //��������˾�
                }
            });
    }
    void SaveCloudData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
           //���� ���� �޼��� Ȥ�� �˾� ��� ����ϰų� �н�
        }
        else
            Debug.Log("!!!!!!!!!!Ŭ���� ���� ����!!!!!!!!!!!!");
    }
    public void LoadData()
    {
        SavedGame().OpenWithAutomaticConflictResolution("UserSaveData", DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (status, data) =>
            {
                //if (status == SavedGameRequestStatus.Success)
                    SavedGame().ReadBinaryData(data, LoadCloudData);
            });
    }
    void LoadCloudData(SavedGameRequestStatus status, byte[] loadData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (loadData != null && loadData.Length > 0)
            {
                string data = System.Text.Encoding.UTF8.GetString(loadData);
                SaveDatas.Load(data);
            }
            else
            {
                SaveDatas.LoadFail();
            }
        }
        else
        {
            SaveDatas.LoadFail();
            Debug.Log("!!!!!!Ŭ���� ������ �ε� ����!!!!!!!");
        }
        isReady = true;
    }

    public void DeleteData()
    {
        SavedGame().OpenWithAutomaticConflictResolution("UserSaveData", DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, (status, data) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    SavedGame().Delete(data);
                }
                else
                    Debug.Log("!!!!!!!!!!!!!Ŭ���� ������ ���� ����!!!!!!!!!!!!!!");
            });
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator Coroutine_AutoSave()
    {
        float time = 0f;
        while(true)
        {
            time += Time.deltaTime;
            if(time > 60)
            {
                time = 0f;
                SaveDatas.SaveServer();
            }
        }
    }
    
}

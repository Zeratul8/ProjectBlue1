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
                    //저장실패팝업
                }
            });
    }
    void SaveCloudData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
           //저장 성공 메세지 혹은 팝업 등등 출력하거나 패스
        }
        else
            Debug.Log("!!!!!!!!!!클라우드 저장 실패!!!!!!!!!!!!");
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
            Debug.Log("!!!!!!클라우드 데이터 로딩 실패!!!!!!!");
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
                    Debug.Log("!!!!!!!!!!!!!클라우드 데이터 삭제 실패!!!!!!!!!!!!!!");
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

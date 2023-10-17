using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GPGSSaveData : SingletonMonoBehaviour<GPGSSaveData>
{
    public ISavedGameClient SavedGame()
    {
        return PlayGamesPlatform.Instance.SavedGame;
    }

    public void SaveData(string jsonData)
    {
        SavedGame().OpenWithAutomaticConflictResolution("usersavedata", DataSource.ReadCacheOrNetwork,
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
        SavedGame().OpenWithAutomaticConflictResolution("usersavedata", DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (status, data) =>
            {
                if (status == SavedGameRequestStatus.Success)
                    SavedGame().ReadBinaryData(data, LoadCloudData);
            });
    }
    void LoadCloudData(SavedGameRequestStatus status, byte[] loadData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string data = System.Text.Encoding.UTF8.GetString(loadData);
            // ���⼭ ���� �ҷ��� ������ ��Ʈ�� ������ Ŭ�����°� �Ľ����ָ� �ɵ�
            SaveDatas.Load(data);
        }
        else
        {
            SaveDatas.LoadFail();
            Debug.Log("!!!!!!Ŭ���� ������ �ε� ����!!!!!!!");
        }
    }

    public void DeleteData()
    {
        SavedGame().OpenWithAutomaticConflictResolution("usersavedata", DataSource.ReadCacheOrNetwork,
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
    
}

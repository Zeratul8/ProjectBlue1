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
           //저장 성공 메세지 혹은 팝업 등등 출력하거나 패스
        }
        else
            Debug.Log("!!!!!!!!!!클라우드 저장 실패!!!!!!!!!!!!");
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
            // 여기서 이제 불러온 데이터 스트링 가지고 클래스맞게 파싱해주면 될듯
            SaveDatas.Load(data);
        }
        else
        {
            SaveDatas.LoadFail();
            Debug.Log("!!!!!!클라우드 데이터 로딩 실패!!!!!!!");
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
                    Debug.Log("!!!!!!!!!!!!!클라우드 데이터 삭제 실패!!!!!!!!!!!!!!");
            });
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDatas : MonoBehaviour
{
    GameData Data;

    public void ReceiveData(int currentStage, float currentGold, int currentDiamond)
    {
        Data.etc.stage = currentStage;
        Data.etc.gold= currentGold;
        Data.etc.diamond= currentDiamond;
    }
    private void GameDatasSave()
    {
        // 데이터 저장
    }
}
class GameData
{
    public PlayerStatus stat;
    public EtcData etc;
}
class EtcData
{
    public int stage;
    public float gold;
    public int diamond;
    // 추가될 정보들은 아래로 적기
}

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
        // ������ ����
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
    // �߰��� �������� �Ʒ��� ����
}

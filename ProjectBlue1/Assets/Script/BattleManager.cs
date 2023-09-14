using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    public enum RoleType
    {
        None,
        Player,
        Monster,
        Max
    }
    [SerializeField]
    PlayerController player;
    [SerializeField]
    MonsterController monster;

    PlayerBattleController playerBattle;
    MonsterBattleController monsterBattle;

    public bool isPlayerAttack = false;
    protected override void OnAwake()
    {
        DataManager.Instance.InitMonsterData();
        DataManager.Instance.InitPlayerData();
    }
    private void Start()
    {
        if(monster == null)
        {
            monster = MonsterPoolManager.Instance.GetMonster();
        }
        //플레이어는 그대로, 몬스터만 풀에서 갱신해서 받아오게 바꾸기

        playerBattle = player.GetComponent<PlayerBattleController>();
        monsterBattle = monster.GetComponent<MonsterBattleController>();

        StartBattle();
    }
    public void ProcessAttack(RoleType type, float attack)
    {
        switch (type)
        {
            case RoleType.Player:
                monsterBattle.Damaged(attack);
                Debug.Log("!!!!!!!플레이어가 공격햇따!!!!!!!!");
                break;
            case RoleType.Monster:
                playerBattle.Damaged(attack);
                Debug.Log("!!!!!!!몬스터가 공격햇따!!!!!!!!");
                break;
            default:
                break;
        }
    }

    public void StartBattle()
    {
        DataManager.Instance.InitPlayerData();
        DataManager.Instance.InitMonsterData();
        //monster = MonsterPoolManager.Instance.GetMonster();
        player.InitControlPlayer();
        monster.InitControlMonster();
        playerBattle.InitBattlePlayer();
        monsterBattle.InitBattleMonster();
    }

    public void KillMonster()
    {
        EndStage();
    }
    public void EndStage()
    {
        GetBattleRewards();
        player.ResetBattleCondition();
        SaveDatas.Data.etc.stage++;
        MonsterPoolManager.Instance.SetMonster(monster);

        SetBattleMonster();
    }
    void GetBattleRewards()
    {
        SaveDatas.Data.etc.gold += SaveDatas.Data.etc.stage;
    }
    void SetBattleMonster()
    {
        monster = MonsterPoolManager.Instance.GetMonster();
        monster.InitMonster();
        monsterBattle = monster.GetComponent<MonsterBattleController>();
        monsterBattle.InitBattleMonster();
    }



}

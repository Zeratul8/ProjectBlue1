using GooglePlayGames;
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
    enum BattleState
    {
        None,
        InitBattle,
        MonsterReady,
        MonsterWait,
        StartBattle,
        PlayBattle,
        EndBattle,
        AfterBattle,
        Max
    }
    [SerializeField]
    PlayerController player;
    [SerializeField]
    MonsterController monster;
    [SerializeField]
    BackGroundMove backGround;

    PlayerBattleController playerBattle;
    MonsterBattleController monsterBattle;

    BattleState battleState = BattleState.InitBattle;

    float[] cristalPriority = { 99, 0.4f, 0.3f, 0.2f, 0.1f };
    float[] testCristalPriority = { 0, 25f, 25f, 25f, 25f };

    public bool isPlayerAttack = false;
    
    protected override void OnAwake()
    {
        PlayGamesPlatform.Activate();
        GPGSSaveData.Instance.LoadData();
    }
    private void Start()
    {
        if (monster == null)
        {
            monster = MonsterPoolManager.Instance.GetMonster();
        }
        //플레이어는 그대로, 몬스터만 풀에서 갱신해서 받아오게 바꾸기

        playerBattle = player.GetComponent<PlayerBattleController>();
        monsterBattle = monster.GetComponent<MonsterBattleController>();

        
    }
    private void Update()
    {
        switch (battleState)
        {
            case BattleState.InitBattle:
                InitBattle();
                battleState = BattleState.MonsterReady;
                break;
            case BattleState.MonsterReady:
                SetBattleMonster();
                battleState= BattleState.MonsterWait;
                break;
            case BattleState.MonsterWait:
                break;
            case BattleState.StartBattle:
                StartBattle();
                battleState = BattleState.PlayBattle;
                break;
            case BattleState.PlayBattle:
                break;
            case BattleState.EndBattle:
                KillMonster();
                break;
            case BattleState.AfterBattle:
                break;
            default:
                break;

        }
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

    public void SetPlayerStat()
    {
        playerBattle.SetPlayerStat();
    }

    public void SetBattleStart()
    {
        battleState = BattleState.StartBattle;
    }
    public void SetBattleEnd()
    {
        battleState = BattleState.EndBattle;
    }

    public void InitBattle()
    {
        //monster = MonsterPoolManager.Instance.GetMonster();
        player.InitControlPlayer();
        monster.InitControlMonster();

        playerBattle.SetPlayerStat();
        //playerBattle.InitBattlePlayer();
        //monsterBattle.InitBattleMonster();
        //monster.InitMonster();
    }

    public void StartBattle()
    {
        monsterBattle.InitBattleMonster();
        playerBattle.InitBattlePlayer();
        backGround.StopBackGroundScrolling();
    }

    public void KillMonster()
    {
        StartCoroutine(EndStage());
    }
    IEnumerator EndStage()
    {
        Debug.Log("!!!!!!!!!몬스터 게임오브젝트이름1 : " + monster.gameObject.name);
        battleState = BattleState.AfterBattle;
        //monster.DieAnimation();
        GetBattleRewards();
        playerBattle.EndBattlePlayer();
        yield return null;
        Debug.Log("!!!!!!!!!몬스터 게임오브젝트이름2 : " + monster.gameObject.name);
        player.ResetBattleCondition(player.playerType);

        if(!UIManager.Instance.repeatMode)
            SaveDatas.Data.etc.stage++;
        
        SaveDatas.SetHighestStage();
        UIManager.Instance.SetStateText();
        MonsterPoolManager.Instance.SetMonster(monster);
        monster = null;

        battleState = BattleState.MonsterReady;
    }
    void GetBattleRewards()
    {
        SaveDatas.Data.etc.gold += SaveDatas.Data.etc.stage;
        UIManager.Instance.SetGoldText();
        GetCristal();
        UIManager.Instance.SetCristalText();
    }
    void SetBattleMonster()
    {
        if(monster == null)
            monster = MonsterPoolManager.Instance.GetMonster();
        monster.InitControlMonster();
        monsterBattle = monster.GetComponent<MonsterBattleController>();
        //monsterBattle.InitBattleMonster();
        monster.InitMonster();
        backGround.StartBackGroundScrolling();
    }
    void GetCristal()
    {
        int priority = Util.GetPriority(cristalPriority);
        if (priority == 1)
        {
            SaveDatas.Data.etc.cristal += 1;
        }
        else if (priority == 2)
        {
            SaveDatas.Data.etc.cristal += 2;
        }
        else if (priority == 3)
        {
            SaveDatas.Data.etc.cristal += 3;
        }
        else if (priority == 4)
        {
            SaveDatas.Data.etc.cristal += 4;
        }
        else
        {

        }
    }



}

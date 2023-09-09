using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        if(monster == null)
        {
            monster = MonsterPoolManager.Instance.GetMonster();
        }
        //플레이어는 그대로, 몬스터만 풀에서 갱신해서 받아오게 바꾸기
        playerBattle = player.GetComponent<PlayerBattleController>();
        monsterBattle = monster.GetComponent<MonsterBattleController>();
        
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

    public void KillMonster()
    {
        EndStage();
    }
    public void EndStage()
    {
        GetBattleRewards();
        player.ResetBattleCondition();
        //MonsterPoolManager.Instance.SetMonster(monster);

        //SetBattleMonster();
    }
    void GetBattleRewards()
    {

    }
    void SetBattleMonster()
    {
        monster = MonsterPoolManager.Instance.GetMonster();
        
        //여기에 몬스터 걸어오는 애니메이션 실행시켜야함
    }



}

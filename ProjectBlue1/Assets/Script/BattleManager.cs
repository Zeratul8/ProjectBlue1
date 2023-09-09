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
        //�÷��̾�� �״��, ���͸� Ǯ���� �����ؼ� �޾ƿ��� �ٲٱ�
        playerBattle = player.GetComponent<PlayerBattleController>();
        monsterBattle = monster.GetComponent<MonsterBattleController>();
        
    }

    public void ProcessAttack(RoleType type, float attack)
    {
        switch (type)
        {
            case RoleType.Player:
                monsterBattle.Damaged(attack);
                Debug.Log("!!!!!!!�÷��̾ �����޵�!!!!!!!!");
                break;
            case RoleType.Monster:
                playerBattle.Damaged(attack);
                Debug.Log("!!!!!!!���Ͱ� �����޵�!!!!!!!!");
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
        
        //���⿡ ���� �ɾ���� �ִϸ��̼� ������Ѿ���
    }



}

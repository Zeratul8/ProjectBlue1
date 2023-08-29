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
    PlayerController monster;

    PlayerBattleController playerBattle;
    MonsterBattleController monsterBattle;

    public bool isPlayerAttack = false;

    private void Start()
    {
        //�÷��̾�� �״��, ���͸� Ǯ���� �����ؼ� �޾ƿ��� �ٲٱ�
        playerBattle = player.GetComponent<PlayerBattleController>();
        monsterBattle = monster.GetComponent<MonsterBattleController>();
    }


    private void Update()
    {

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
    
}

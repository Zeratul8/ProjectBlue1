using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;

    MonsterStatus monStat;
    MonsterBattleController monsterBattle;

    public MonsterType monType;

    [SerializeField]
    private AnimationController aniController;
    public enum MonsterType
    {
        None = -1,
        Skeleton,
        SkeletonWarrior,
        SkeletonMage,
        SkeletonArchor,
        Max
    }
    private void Start()
    {
        aniController = GetComponentInChildren<AnimationController>();
        monsterBattle = GetComponent<MonsterBattleController>();
    }
    public void InitControlMonster()
    {
        if(TryGetComponent<MonsterStatus>(out monStat) == false)
        {
            monStat = gameObject.AddComponent<MonsterStatus>();
        }
        if (GetComponent<MonsterBattleController>() == null)
        {
            gameObject.AddComponent<MonsterBattleController>();
        }
        monStat.InitFirstStats();
        InitMonster();
    }

    public void InitMonster()
    {
        StartCoroutine(Coroutine_MonsterWalk());
    }

    public void AttackAnimation(MonsterType monType)
    {
        aniController.Attack_Animation(monType);
    }


    IEnumerator Coroutine_MonsterWalk()
    {
        //�ִϸ��̼� ����ٳ���
        aniController.State_Animation(monType);

        Vector3 walkPos = new Vector3(-walkSpeed, 0, 0);
        while(true)
        {
            yield return null;
            transform.position += walkPos;
            if (transform.position.x < 3.45f)
            {
                // ��ġ�� �������� �� �������� ���߰� �÷��̾ ������
                aniController.State_Animation();
                StartCoroutine(monsterBattle.AttackSpeed_Bar());
                break;
            }
        }
    }
}

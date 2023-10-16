using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{

    MonsterStatus monStat;
    MonsterBattleController monsterBattle;
    Transform body;

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
        Constants.monsterHeight = GetComponent<CapsuleCollider2D>().size.y;
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
    }

    public void InitMonster()
    {
        aniController.ResetStand_Animation();
        StartCoroutine(Coroutine_MonsterWalk());
    }

    public void AttackAnimation(MonsterType monType)
    {
        aniController.Attack_Animation(monType);
    }
    public void DieAnimation()
    {
        aniController.Die_Animation();
    }


    IEnumerator Coroutine_MonsterWalk()
    {
        //애니메이션 여기다넣자
        aniController.State_Animation(monType);

        Vector3 walkPos = new Vector3(-Constants.monsterWalkSpeed, 0, 0) * Time.deltaTime;
        while(true)
        {
            yield return null;
            transform.position += walkPos;
            if (transform.position.x < 3.45f)
            {
                // 위치에 도달했을 때 움직임을 멈추고 플레이어를 공격함
                aniController.StopWalk_Animation();
                //StartCoroutine(monsterBattle.AttackSpeed_Bar());
                BattleManager.Instance.SetBattleStart();
                break;
            }
        }
    }
}

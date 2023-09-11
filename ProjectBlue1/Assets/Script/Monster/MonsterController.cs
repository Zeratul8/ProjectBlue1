using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;

    MonsterStatus monStat;

    public MonsterType monType;
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
        transform.position = Constants.monsterPos;
        StartCoroutine(Coroutine_MonsterWalk());
    }


    IEnumerator Coroutine_MonsterWalk()
    {
        //애니메이션 여기다넣자
        aniController.Action_Animation(monType);

        Vector3 walkPos = new Vector3(-walkSpeed, 0, 0);
        while(true)
        {
            yield return null;
            transform.position += walkPos;
            if (transform.position.x < 3.45f)
                break;
        }
    }
}

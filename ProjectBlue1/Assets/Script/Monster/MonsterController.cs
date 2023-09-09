using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;

    MonsterStatus monStat;
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
        transform.position = new Vector3(6.9f, 0, 0);

    }

    IEnumerator Coroutine_MonsterWalk()
    {
        Vector3 walkPos = new Vector3(-walkSpeed, 0, 0);
        while(true)
        {
            yield return null;
            transform.position += walkPos;
            if (Mathf.Approximately(transform.position.x, 3.45f))
                break;
        }
    }
}

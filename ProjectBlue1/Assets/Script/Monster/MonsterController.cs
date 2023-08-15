using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
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
        monStat.InitFirstStats();
    }
}

using Assets.HeroEditor.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : SingletonMonoBehaviour<MonsterPoolManager>
{
    [SerializeField]
    GameObject monsterObj;
    [SerializeField]
    Pooling monPool;

    protected override void OnStart()
    {
        if(monPool == null)
        {
            monPool = gameObject.AddComponent<Pooling>();
        }
        if(monsterObj == null)
        {
            monsterObj = new GameObject("Monster");
            monsterObj.AddComponent<MonsterController>();
        }
        monsterObj.SetActive(false);

        monPool.maxSize = 10;
        monPool.element = monsterObj;
        monPool.Init();
    }

    //풀링 손봐야함..
    public MonsterController GetMonster()
    {
        GameObject monster = (GameObject)monPool.Get_Element();
        monster.SetActive(true);
        return monster.GetComponent<MonsterController>();
    }

    public void SetMonster(MonsterController monster)
    {
        monster.gameObject.SetActive(false);
        monPool.Returned_Element(monster);
    }
}

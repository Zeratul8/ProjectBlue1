using Assets.HeroEditor.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : SingletonMonoBehaviour<MonsterPoolManager>
{
    [SerializeField]
    GameObject monsterObj;
    //[SerializeField]
    //Pooling monPool;

    GameObjectPool<MonsterController> monsterPool;

    protected override void OnStart()
    {
        /*if(monPool == null)
        {
            monPool = gameObject.AddComponent<Pooling>();
        }*/
        if(monsterObj == null)
        {
            monsterObj = new GameObject("Monster");
            monsterObj.AddComponent<MonsterController>();
        }
        monsterObj.SetActive(false);

        monsterPool = new GameObjectPool<MonsterController>(5, () =>
        {
            GameObject obj = Instantiate(monsterObj, this.transform);
            obj.transform.position = Constants.monsterPos;
            MonsterController monCtr = obj.GetComponent<MonsterController>();
            obj.SetActive(false);
            return monCtr;
        });





        /*monPool.maxSize = 10;
        monPool.element = monsterObj;
        monPool.Init();*/
    }
    public MonsterController GetMonster()
    {
        MonsterController mon = monsterPool.Get();
        mon.gameObject.SetActive(true);
        return mon;
    }
    public void SetMonster(MonsterController mon)
    {
        mon.gameObject.SetActive(false);
        mon.transform.position = Constants.monsterPos;
        monsterPool.Set(mon);
    }




    /*
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
    */
}

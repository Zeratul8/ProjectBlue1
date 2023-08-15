using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterBattleController : MonoBehaviour, IBattleController
{
    MonsterStatus stat;

    void Start()
    {
        stat = GetComponent<MonsterStatus>();
    }

    public void Attack()
    {
        
    }
    public void Damaged()
    {

    }
    public void Die()
    {

    }
}

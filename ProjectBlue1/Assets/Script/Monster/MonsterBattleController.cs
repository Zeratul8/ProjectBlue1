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

    public void Attack(float attack)
    {
        
    }
    public void Damaged(float damage)
    {
        stat.MonStat.Health -= damage;
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
}

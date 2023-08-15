using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    PlayerStatus stat;

    private void Start()
    {
        stat = GetComponent<PlayerStatus>();
    }
    public void Attack(float attack)
    {
        _monster.MonStat.Health -= attack;
    }
    public void Damaged(float damage)
    {
        stat.PlayerStat.Health -= damage;
    }
    public void Die()
    {
        if(stat.PlayerStat.Health <= 0)
        {
            // 부활 처리
        }
    }
}

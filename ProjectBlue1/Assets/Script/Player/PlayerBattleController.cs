using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    PlayerStatus stat;

    [SerializeField] private Image attackSpeedBar;
    private void Start()
    {
        stat = GetComponent<PlayerStatus>();
    }
    private void Update()
    {
        AttackSpeed_Bar();
    }
    // 스텟 초기화
    public void StatInit()
    {
        
    }
    // 게이지 채우기
    public void AttackSpeed_Bar()
    {
        if(attackSpeedBar.fillAmount >= 1)
        {
            Debug.Log("공격함!!");
            attackSpeedBar.fillAmount = 0;
        }
        else
        {
            attackSpeedBar.fillAmount += (1f/stat.PlayerStat.AttackSpeed) * Time.deltaTime;
        }
    }
    public void Attack(ref float health)
    {
        health -= stat.PlayerStat.Attack;

        /* 또는
        반환값을 float으로 해두고
        return health;
        */
    }
    public void Damaged(float damage)
    {
        stat.PlayerStat.Health -= damage;
    }
    public void Die()
    {
        if(stat.PlayerStat.Health <= 0)
        {
            // 부활 처리 ? 스테이지 다시 시작?
        }
    }
}

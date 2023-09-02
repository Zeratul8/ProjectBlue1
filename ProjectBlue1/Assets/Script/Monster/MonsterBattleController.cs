using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterBattleController : MonoBehaviour//, IBattleController
{
    //MonsterStatus stat;
    PlayerStatus stat;

    [SerializeField] private Slider attackSpeedBar;
    /*[SerializeField]*/
    private AnimationController aniController;
    private void Start()
    {
        stat = GetComponent<PlayerStatus>();
        aniController = GetComponentInChildren<AnimationController>();
        
        StartCoroutine(AttackSpeed_Bar());
    }


    public void Attack()
    {
        BattleManager.Instance.ProcessAttack(BattleManager.RoleType.Monster, 0.000001f);
    }
    public void Damaged(float damage)
    {
        //stat.MonStat.Health -= damage;
        stat.playerStat.Health -= damage;
        Debug.Log("!!!!!!몬스터남은피 : " + stat.playerStat.Health + "!!!!!!");
        if (stat.playerStat.Health <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("!!!!!!몬스터가쥬것따!!!!!!!");
        gameObject.SetActive(false);
    }


    private IEnumerator AttackSpeed_Bar()
    {
        while (true)
        {
            // 현재 진행된 공격 속도 게이지가 최대가 되었다면 공격 후 0으로 초기화
            if (attackSpeedBar.value >= attackSpeedBar.maxValue)
            {
                aniController.Action_Animation();
                Attack();
                Debug.Log("공격함!");
                attackSpeedBar.value = 0;
            }
            else
            {
                attackSpeedBar.value += (attackSpeedBar.maxValue / stat.playerStat.AttackSpeed) * Time.deltaTime;
                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterBattleController : MonoBehaviour//, IBattleController
{
    public MonsterController.MonsterType monType;
    public ParticleSystem particleSystem;
    [SerializeField]
    MonsterStatus stat;


    [SerializeField] private Slider attackSpeedBar;
    /*[SerializeField]*/
    private AnimationController aniController;
    public void InitBattleMonster()
    {
        stat.InitFirstStats();
        aniController = GetComponentInChildren<AnimationController>();
        StartCoroutine(AttackSpeed_Bar());
    }


    public void Attack()
    {
        BattleManager.Instance.ProcessAttack(BattleManager.RoleType.Monster, 0.000001f);
    }
    public void Damaged(float damage)
    {
        stat.MonStat.Health -= damage;
        Debug.Log("!!!!!!몬스터남은피 : " + stat.MonStat.Health + "!!!!!!");
        if (stat.MonStat.Health <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("!!!!!!몬스터가쥬것따!!!!!!!");
        // 몬스터가 살아있을 때 한 번만 파티클 이펙트 발동
        if (gameObject.activeSelf)
        {
            particleSystem.Play();
        }
        StopAllCoroutines();
        attackSpeedBar.value = 0;
        BattleManager.Instance.KillMonster();
    }

    public void StartBattleMonster()
    {
        
    }


    private IEnumerator AttackSpeed_Bar()
    {
        while (true)
        {
            // 현재 진행된 공격 속도 게이지가 최대가 되었다면 공격 후 0으로 초기화
            if (attackSpeedBar.value >= attackSpeedBar.maxValue)
            {
                aniController.Action_Animation(monType);
                Attack();
                Debug.Log("공격함!");
                attackSpeedBar.value = 0;
            }
            else
            {
                attackSpeedBar.value += (attackSpeedBar.maxValue / stat.MonStat.AttackSpeed) / 2 * Time.deltaTime;
                yield return null;
            }
        }
    }
}

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


    [SerializeField]
    private Slider attackSpeedBar;

    /*[SerializeField]*/
    private AnimationController aniController;
    public void InitBattleMonster()
    {
        stat.InitFirstStats();
        Debug.Log(stat.MonStat.Health);
        aniController = GetComponentInChildren<AnimationController>();
    }


    public void Attack()
    {
        BattleManager.Instance.ProcessAttack(BattleManager.RoleType.Monster, 0.000001f);
    }
    public void Damaged(float damage)
    {
        stat.MonStat.Health -= damage;
        Debug.Log("!!!!!!���ͳ����� : " + stat.MonStat.Health + "!!!!!!");
        if (stat.MonStat.Health <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("!!!!!!���Ͱ���͵�!!!!!!!");
        // ���Ͱ� ������� �� �� ���� ��ƼŬ ����Ʈ �ߵ�
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


    public IEnumerator AttackSpeed_Bar()
    {
        while (true)
        {
            // ���� ����� ���� �ӵ� �������� �ִ밡 �Ǿ��ٸ� ���� �� 0���� �ʱ�ȭ
            if (attackSpeedBar.value >= attackSpeedBar.maxValue)
            {
                aniController.Action_Animation(monType);
                Attack();
                Debug.Log("������!");
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

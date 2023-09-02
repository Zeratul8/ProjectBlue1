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
        Debug.Log("!!!!!!���ͳ����� : " + stat.playerStat.Health + "!!!!!!");
        if (stat.playerStat.Health <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("!!!!!!���Ͱ���͵�!!!!!!!");
        gameObject.SetActive(false);
    }


    private IEnumerator AttackSpeed_Bar()
    {
        while (true)
        {
            // ���� ����� ���� �ӵ� �������� �ִ밡 �Ǿ��ٸ� ���� �� 0���� �ʱ�ȭ
            if (attackSpeedBar.value >= attackSpeedBar.maxValue)
            {
                aniController.Action_Animation();
                Attack();
                Debug.Log("������!");
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

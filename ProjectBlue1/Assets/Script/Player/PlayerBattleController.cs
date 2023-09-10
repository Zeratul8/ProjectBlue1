using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    [SerializeField]
    PlayerStatus stat;

    public ParticleSystem particleSystem;
    [SerializeField] private Slider attackSpeedBar;
    /*[SerializeField]*/ private AnimationController aniController;
    public void InitBattlePlayer()
    {
        
        aniController = GetComponentInChildren<AnimationController>();
        StartCoroutine(AttackSpeed_Bar());
    }
    // ���� �ʱ�ȭ
    public void StatInit()
    {
        // ������ ���� ���� ������ �����ͼ� �ʱ�ȭ �ؾ��Ұ� ������, ���� ���� ������ ��� �����;� ���� �𸣰ڽ��ϴ�..
    }
    // ������ ä���
    
    public void Attack()
    {
        BattleManager.Instance.ProcessAttack(BattleManager.RoleType.Player, stat.playerStat.Attack);
    }
    public void Damaged(float damage)
    {
        stat.playerStat.Health -= damage;
        if (stat.playerStat.Health < 0)
            Die();
    }
    public void Die()
    {
        if(stat.playerStat.Health <= 0)
        {
            Debug.Log("!!!!!!�÷��̾ ��͵�!!!!!!");
            // ��Ȱ ó�� ? �������� �ٽ� ����?
        }
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

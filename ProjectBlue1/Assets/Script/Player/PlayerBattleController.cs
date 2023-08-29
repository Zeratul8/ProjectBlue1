using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    PlayerStatus stat;

    [SerializeField] private Slider attackSpeedBar;
    /*[SerializeField]*/ private AnimationController aniController;
    private void Start()
    {
        stat = GetComponent<PlayerStatus>();
        aniController = GetComponentInChildren<AnimationController>();
    }
    private void Update()
    {
        AttackSpeed_Bar();
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



    private void AttackSpeed_Bar()
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
            attackSpeedBar.value += (attackSpeedBar.maxValue / stat.playerStat.AttackSpeed) * Time.deltaTime*5;
        }
    }
}

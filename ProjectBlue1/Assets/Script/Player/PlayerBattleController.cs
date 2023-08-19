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
    // ���� �ʱ�ȭ
    public void StatInit()
    {
        
    }
    // ������ ä���
    public void AttackSpeed_Bar()
    {
        if(attackSpeedBar.fillAmount >= 1)
        {
            Debug.Log("������!!");
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

        /* �Ǵ�
        ��ȯ���� float���� �صΰ�
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
            // ��Ȱ ó�� ? �������� �ٽ� ����?
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    PlayerStatus stat;

    [SerializeField] private Slider attackSpeedBar;
    [SerializeField] private AnimationController _animator;
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
        // ������ ���� ���� ������ �����ͼ� �ʱ�ȭ �ؾ��Ұ� ������, ���� ���� ������ ��� �����;� ���� �𸣰ڽ��ϴ�..
    }
    // ������ ä���
    public void AttackSpeed_Bar()
    {
        // ���� ����� ���� �ӵ� �������� �ִ밡 �Ǿ��ٸ� ���� �� 0���� �ʱ�ȭ
        if(attackSpeedBar.value >= attackSpeedBar.maxValue)
        {
            _animator.Action_Animation();
            Debug.Log("������!");
            attackSpeedBar.value = 0;
        }
        else
        {
            attackSpeedBar.value += (1f / stat.PlayerStat.AttackSpeed) * Time.deltaTime;
        }
    }
    public float Attack(float health)
    {
        health -= stat.PlayerStat.Attack;

        return health;
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

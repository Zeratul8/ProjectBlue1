using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    [SerializeField]
    PlayerController.PlayerType playerType;
    [SerializeField]
    PlayerController playerCtr;

    public ParticleSystem particleSystem;
    [SerializeField] private Slider attackSpeedBar;
    public void InitBattlePlayer()
    {
        StopAllCoroutines();
        playerCtr.StopWalkPlayer();
        StartCoroutine(AttackSpeed_Bar());
    }
    // ���� �ʱ�ȭ
    public void StatInit()
    {
        // ������ ���� ���� ������ �����ͼ� �ʱ�ȭ �ؾ��Ұ� ������, ���� ���� ������ ��� �����;� ���� �𸣰ڽ��ϴ�..
    }

    public void SetPlayerStat()
    {
        PlayerStatus.playerStat.Attack = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.AttackLv].Attack;
        PlayerStatus.playerStat.Health = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.HealthLv].Health;
        PlayerStatus.playerStat.CriticalHit = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.CriticalHitLv].CriticalHit;
        PlayerStatus.playerStat.CriticalDamage = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.CriticalDamageLV].CriticalDamage;
        PlayerStatus.playerStat.AttackSpeed = DataManager.Instance.PlayerStats[SaveDatas.Data.stat.AttackSpeedLv].AttackSpeed;
    }

    
    public void Attack()
    {
        BattleManager.Instance.ProcessAttack(BattleManager.RoleType.Player, PlayerStatus.playerStat.Attack);
    }
    public void Damaged(float damage)
    {
        PlayerStatus.playerHP -= damage;
        if (PlayerStatus.playerHP < 0)
            Die();
    }
    public void Die()
    {
        if(PlayerStatus.playerHP <= 0)
        {
            Debug.Log("!!!!!!�÷��̾ ��͵�!!!!!!");
            // ��Ȱ ó�� ? �������� �ٽ� ����?
        }
    }
    public void EndBattlePlayer()
    {
        StopAllCoroutines();
    }


    private IEnumerator AttackSpeed_Bar()
    {
        while (true)
        {
            // ���� ����� ���� �ӵ� �������� �ִ밡 �Ǿ��ٸ� ���� �� 0���� �ʱ�ȭ
            if (attackSpeedBar.value >= attackSpeedBar.maxValue)
            {
                playerCtr.AttackAnimation(playerType);
                EffectManager.Instance.SlashSword(true);
                Shooter.Instance.GetProjectile();
                //Attack();
                Debug.Log("������!");
                attackSpeedBar.value = 0;
            }
            else
            {
                attackSpeedBar.value += (attackSpeedBar.maxValue / PlayerStatus.playerStat.AttackSpeed) * Time.deltaTime;
                yield return null;
            }
        }
    }
}

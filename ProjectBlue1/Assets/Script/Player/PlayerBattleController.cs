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
    // 스텟 초기화
    public void StatInit()
    {
        // 스텟을 현재 유저 정보를 가져와서 초기화 해야할거 같은데, 현재 유저 정보를 어떻게 가져와야 할지 모르겠습니당..
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
            Debug.Log("!!!!!!플레이어가 쥬것따!!!!!!");
            // 부활 처리 ? 스테이지 다시 시작?
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
            // 현재 진행된 공격 속도 게이지가 최대가 되었다면 공격 후 0으로 초기화
            if (attackSpeedBar.value >= attackSpeedBar.maxValue)
            {
                playerCtr.AttackAnimation(playerType);
                EffectManager.Instance.SlashSword(true);
                Shooter.Instance.GetProjectile();
                //Attack();
                Debug.Log("공격함!");
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

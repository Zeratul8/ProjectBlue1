using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    [SerializeField]
    PlayerController.PlayerType playerType;
    [SerializeField]
    PlayerStatus stat;
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
        stat.playerStat.Attack = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackLv].Attack;
        stat.playerStat.Health = DataManager.Instance.playerStats[SaveDatas.Data.stat.HealthLv].Health;
        stat.playerStat.CriticalHit = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalHitLv].CriticalHit;
        stat.playerStat.CriticalDamage = DataManager.Instance.playerStats[SaveDatas.Data.stat.CriticalDamageLV].CriticalDamage;
        stat.playerStat.AttackSpeed = DataManager.Instance.playerStats[SaveDatas.Data.stat.AttackSpeedLv].AttackSpeed;
    }

    
    public void Attack()
    {
        BattleManager.Instance.ProcessAttack(BattleManager.RoleType.Player, stat.playerStat.Attack);
    }
    public void Damaged(float damage)
    {
        stat.playerHP -= damage;
        if (stat.playerHP < 0)
            Die();
    }
    public void Die()
    {
        if(stat.playerHP <= 0)
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

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
    // 스텟 초기화
    public void StatInit()
    {
        // 스텟을 현재 유저 정보를 가져와서 초기화 해야할거 같은데, 현재 유저 정보를 어떻게 가져와야 할지 모르겠습니당..
    }
    // 게이지 채우기
    
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
            Debug.Log("!!!!!!플레이어가 쥬것따!!!!!!");
            // 부활 처리 ? 스테이지 다시 시작?
        }
    }



    private void AttackSpeed_Bar()
    {
        // 현재 진행된 공격 속도 게이지가 최대가 되었다면 공격 후 0으로 초기화
        if (attackSpeedBar.value >= attackSpeedBar.maxValue)
        {
            aniController.Action_Animation();
            Attack();
            Debug.Log("공격함!");
            attackSpeedBar.value = 0;
        }
        else
        {
            attackSpeedBar.value += (attackSpeedBar.maxValue / stat.playerStat.AttackSpeed) * Time.deltaTime*5;
        }
    }
}

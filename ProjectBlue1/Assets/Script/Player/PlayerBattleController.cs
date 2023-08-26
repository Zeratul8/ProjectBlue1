using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour, IBattleController
{
    PlayerStatus stat;

    [SerializeField] private Image attackSpeedBar;
    [SerializeField] private AnimationController _animator;
    private void Start()
    {
        stat = GetComponent<PlayerStatus>();
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
    public void AttackSpeed_Bar()
    {
        if(attackSpeedBar.fillAmount >= 1)
        {
            _animator.Action_Animation();
            Debug.Log("공격함!");
            attackSpeedBar.fillAmount = 0;
        }
        else
        {
            attackSpeedBar.fillAmount += (1f/stat.PlayerStat.AttackSpeed) * Time.deltaTime;
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
            // 부활 처리 ? 스테이지 다시 시작?
        }
    }
}

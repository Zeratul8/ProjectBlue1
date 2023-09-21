using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStatus stat;

    public PlayerType playerType;

    AnimationController animCtr;


    public enum PlayerType
    {
        None = -1,
        Warrior,
        Archor,
        Max
    }
    public void InitControlPlayer()
    {
        animCtr = GetComponentInChildren<AnimationController>();
        stat = GetComponent<PlayerStatus>();
        stat.InitFirstStats();
        ResetBattleCondition(playerType);
    }

 
    string FormatNumber(float number)
    {
        if(number >= 1000)
        {
            return (number / 1000f).ToString("0.0K");
        }
        else if(number >= 10000)
        {
            return (number / 10000f).ToString("0.0M");
        }
        else
        {
            return number.ToString();
        }
    }
    public void AttackAnimation(PlayerType playerType)
    {
        animCtr.Attack_Animation(playerType);
    }
    public void DieAnimation()
    {
        animCtr.Die_Animation();
    }
    public void ResetBattleCondition(PlayerType playerType)
    {
        //여기에 플레이어 걸어가는 애니메이션 실행시켜야함
        animCtr.State_Animation(playerType);
    }
    public void StopWalkPlayer()
    {
        animCtr.StopWalk_Animation();
    }

    
}

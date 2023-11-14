using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public enum AniName
    {
        Jab,
        Slash,
        Cast,
        SimpleBowShot,
        DeathFront,
        DeathBack,
        Max
    }
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 몹 타입을 가져와서 해당 몹에 맞는 애니메이션을 사용함

    // Upper 레이어 ( 공격 모션 관련 )
    public void Attack_Animation(MonsterController.MonsterType mobType = MonsterController.MonsterType.None)
    {
        switch (mobType)
        {
            case MonsterController.MonsterType.Skeleton:
                animator.SetTrigger("Jab");
                break;
            case MonsterController.MonsterType.SkeletonWarrior:
                animator.SetTrigger("Slash");
                break;
            case MonsterController.MonsterType.SkeletonMage:
                animator.SetTrigger("Cast");
                break;
            case MonsterController.MonsterType.SkeletonArchor:
                animator.SetTrigger("SimpleBowShot");
                break;
            default:
                Debug.Log("!!!!!AnimationController.Attack_Animation - 유효하지 않은 몬스터 타입!!!!" + gameObject.name);
                break;
        }
    }
    public void Attack_Animation(PlayerController.PlayerType playerType = PlayerController.PlayerType.Warrior)
    {
        switch (playerType)
        {
            case PlayerController.PlayerType.Warrior:
                animator.SetTrigger("Slash");
                break;
            case PlayerController.PlayerType.Archor:
                animator.SetTrigger("SimpleBowShot");
                break;
            default:
                Debug.Log("!!!!!AnimationController.Attack_Animation - 유효하지 않은 플레이어 타입!!!!" + gameObject.name);
                break;
        }
    }
    // Lower 레이어 ( 움직임 관련 )
    public void State_Animation(MonsterController.MonsterType mobType = MonsterController.MonsterType.None)
    {
        switch (mobType)
        {
            case MonsterController.MonsterType.Skeleton:
                animator.SetInteger("State", 2);
                break;
            case MonsterController.MonsterType.SkeletonWarrior:
                //animator.SetTrigger("Slash");
                break;
            case MonsterController.MonsterType.SkeletonMage:
                //animator.SetTrigger("Cast");
                break;
            case MonsterController.MonsterType.SkeletonArchor:
                //animator.SetTrigger("SimpleBowShot");
                break;
            default:
                animator.SetInteger("State", 0);
                break;
        }
    }

    public void State_Animation(PlayerController.PlayerType playerType = PlayerController.PlayerType.Warrior)
    {
        switch(playerType)
        {
            case PlayerController.PlayerType.Warrior:
                animator.SetInteger("State", 2);
                break;
            default:
                Debug.Log("!!!!!AnimationController.State_Animation - 유효하지 않은 플레이어 타입!!!!" + gameObject.name);
                break;
        }
    }

    public void ResetStand_Animation()
    {
        animator.SetInteger("State", 0);
        animator.SetBool("Ready", false);
        animator.SetBool("Action", false);
    }

    /*public void Die_Animation()
    {
        animator.SetInteger("State", Random.Range(6, 8));
    }*/

    public void StopWalk_Animation()
    {
        animator.SetInteger("State", 0);
    }
}

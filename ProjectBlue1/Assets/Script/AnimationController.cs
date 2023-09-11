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
        SimpleBowShot
    }
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // �� Ÿ���� �����ͼ� �ش� ���� �´� �ִϸ��̼��� �����
    public void Action_Animation(MonsterController.MonsterType mobType = MonsterController.MonsterType.None)
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
                animator.SetTrigger("Slash");
                break;
        }
    }
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
                animator.SetTrigger("Slash");
                break;
        }
    }
}

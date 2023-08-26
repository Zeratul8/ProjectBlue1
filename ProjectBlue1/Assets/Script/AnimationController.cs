using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public enum AniType
    {
        Trigger,
        Bool
    }
    public Animator _animator;
    void Start()
    {
        _animator= GetComponent<Animator>();
    }

    // �� Ÿ���� �����ͼ� �ش� ���� �´� �ִϸ��̼��� �����
    public void Action_Animation(MonsterController.MonsterType mobType = MonsterController.MonsterType.None)
    {
        string _mobType = mobType.ToString();
        switch (_mobType)
        {
            case "Skeleton":
                _animator.SetTrigger("Jab");
                break;
            case "SkeletonWarrior":
                _animator.SetTrigger("Slash");
                break;
            case "SkeletonMage":
                _animator.SetTrigger("Cast");
                break;
            case "SkeletonArchor":
                _animator.SetTrigger("SimpleBowShot");
                break;
            default:
                _animator.SetTrigger("Slash");
                break;
        }
    }
}

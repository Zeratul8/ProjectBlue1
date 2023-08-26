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

    // 몹 타입을 가져와서 해당 몹에 맞는 애니메이션을 사용함
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

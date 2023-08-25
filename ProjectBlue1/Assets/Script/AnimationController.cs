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

    public void Action_Animation(string name, AniType type = AniType.Trigger, bool state = false)
    {
        switch (type)
        {
            case AniType.Trigger:
                _animator.SetTrigger(name);
                break;
            case AniType.Bool:
                _animator.SetBool(name, state);
                break;
        }
    }
}

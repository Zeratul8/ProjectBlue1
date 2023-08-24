using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator _animator;
    void Start()
    {
        _animator= GetComponent<Animator>();
    }

    public void Action_Animation(string name)
    {
        _animator.SetTrigger(name);
    }
}

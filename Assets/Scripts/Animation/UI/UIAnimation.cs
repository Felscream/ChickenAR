using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralUI;

[RequireComponent(typeof(Animator))]
public abstract class UIAnimation : MonoBehaviour
{
    protected Animator _animator;
    
    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public abstract void PlayAnimation();
}

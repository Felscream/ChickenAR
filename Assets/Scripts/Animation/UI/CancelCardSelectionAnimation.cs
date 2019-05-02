using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelCardSelectionAnimation : UIAnimation
{
    public bool IsScaledUp = false;

    public override void PlayAnimation()
    {
        IsScaledUp = !IsScaledUp;
        _animator.SetBool("IsHovered", IsScaledUp);
    }

    private void OnDisable()
    {
        IsScaledUp = false;
    }
}

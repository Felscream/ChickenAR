using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardUI
{
    [RequireComponent(typeof(Animator))]
    public class HandAnimation : UIAnimation
    {
        public bool Animated = false;

        public override void PlayAnimation()
        {
            Animated = !Animated;
            _animator.SetBool("Hidden", Animated);
        }

        public void PlayAnimation(bool value)
        {
            Animated = value;
            _animator.SetBool("Hidden", Animated);
        }
    }
}


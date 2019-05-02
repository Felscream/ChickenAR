using System;
using UnityEngine;

namespace GeneralUI
{
    public enum AnimatedElements
    {
        Hand,
        CancelCardSelection
    }

    [Serializable]
    public struct AnimationType
    {
        public AnimatedElements Type;
        public UIAnimation Animation;
    }

    public class UIAnimationController : MonoBehaviour
    {
        public AnimationType[] Animations;

        public UIAnimation GetAnimationByType(AnimatedElements type)
        {
            UIAnimation animation = null;
            for (int i = 0; i < Animations.Length; i++)
            {
                if (Animations[i].Type == type)
                {
                    animation = Animations[i].Animation;
                    break;
                }
            }

            if (animation == null)
            {
                Debug.LogError("Animation of type " + type + " not found", gameObject);
            }

            return animation;
        }

        public void PlayAnimation(AnimatedElements type)
        {
            UIAnimation anim = GetAnimationByType(type);
            if(anim != null)
            {
                anim.PlayAnimation();
            }
        }
    }
}



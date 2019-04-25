using UnityEngine;
using System.Collections;

namespace SA
{
    public class AnimatorData
    {
        public Transform LeftFoot;
        public Transform RightFoot;
        public Transform RightHand;
        public Transform LeftHand;
        public Animator Animator;

        public AnimatorData(Animator anim)
        {
            Animator = anim;
        }
    }
}


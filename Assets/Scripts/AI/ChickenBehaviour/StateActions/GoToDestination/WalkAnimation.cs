using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/GoToDestination/WalkAnimation")]
    public class WalkAnimation : StateActions
    {
        public bool Walk;
        public override void Execute(StateManager states)
        {
            states.Animator.SetBool(states.Hashes.IsWalking, Walk);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/GoToDestination/StartWalking")]
    public class StartWalking : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.IsAtDestination = false;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/GoToDestination/StopWalking")]
    public class StopWalking : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.IsAtDestination = true;
        }
    }
}

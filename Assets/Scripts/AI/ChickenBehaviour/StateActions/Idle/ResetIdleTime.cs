using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    [CreateAssetMenu(menuName = "Actions/State Actions/Idle/ResetIdleTime")]
    public class ResetIdleTime : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.IdleTimer = 0f;
        }
    }
}


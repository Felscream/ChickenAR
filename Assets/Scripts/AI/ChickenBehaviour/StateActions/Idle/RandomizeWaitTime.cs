using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Idle/RandomizeWaitTime")]
    public class RandomizeWaitTime : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.WaitTime = Random.Range(ChickenConstants.WaitIdleRange.rangeStart, ChickenConstants.WaitIdleRange.rangeEnd);
        }
    }
}


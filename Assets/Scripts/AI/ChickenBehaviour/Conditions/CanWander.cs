using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/CanWander")]
    public class CanWander : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state.IdleTimer > state.WaitTime;
        }
    }
}


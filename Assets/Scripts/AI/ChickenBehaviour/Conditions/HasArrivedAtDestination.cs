using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/HasArrived")]
    public class HasArrivedAtDestination : Condition
    {
        public bool HasArrived;
        public override bool CheckCondition(StateManager state)
        {
            return state.IsAtDestination == HasArrived;
        }
    }
}


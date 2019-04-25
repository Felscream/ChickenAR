using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{

    [CreateAssetMenu(menuName = "Actions/State Actions/Idle/Wait")]
    public class Wait : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.IdleTimer += states.delta;
        }
    }
}


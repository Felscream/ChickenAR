using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/ResetRotation")]
    public class ResetRotation : StateActions
    {
        public bool ResetX;
        public bool ResetY;
        public bool ResetZ;

        public override void Execute(StateManager states)
        {
            Vector3 rot = states.transform.rotation.eulerAngles;
            if (ResetX)
                rot.x = 0;
            if (ResetY)
                rot.y = 0;
            if (ResetZ)
                rot.z = 0;

            states.transform.rotation = Quaternion.Euler(rot);
        }
    }
}


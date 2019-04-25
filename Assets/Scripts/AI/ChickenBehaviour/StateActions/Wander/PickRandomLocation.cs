using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Wander/PickRandomLocation")]
    public class PickRandomLocation : StateActions
    {
        public override void Execute(StateManager states)
        {
            Vector3[] bounds = WorldBoundary.Boundaries;
            Vector3 randDest = new Vector3(Random.Range(bounds[0].x, bounds[1].x), 0f, Random.Range(bounds[0].z, bounds[1].z));
            Node target = PathRequestManager.GetNode(randDest);
            if (target.IsWalkable)
            {
                states.GoToDestination(randDest);
            }
        }
    }
}


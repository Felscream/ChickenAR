using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
    public class PathfindingGrid : MonoBehaviour
    {
        public Vector2 GridWorldSize;
        public float NodeRadius;
        public Node[][] grid;
        public LayerMask UnwalkableLayer;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

        }
    }
}


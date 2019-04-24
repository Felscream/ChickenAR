using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding{
    public class Node
    {
        public bool IsWalkable;
        public Vector3 WorldPosition;

        public Node(bool walk, Vector3 wp)
        {
            IsWalkable = walk;
            WorldPosition = wp;
        }
    }
}


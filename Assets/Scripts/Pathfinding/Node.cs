using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding{
    public class Node : IHeapItem<Node>
    {
        public bool IsWalkable;
        public Vector3 WorldPosition;
        public int GridX;
        public int GridY;

        public int GCost;
        public int HCost;
        public Node Parent;

        public int FCost { get { return GCost + HCost; } }

        public Node(bool walk, Vector3 wp, int x, int y)
        {
            IsWalkable = walk;
            WorldPosition = wp;
            GridX = x;
            GridY = y;
        }

        public override int CompareTo(Node other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if(compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }

            return -compare;
        }
    }
}


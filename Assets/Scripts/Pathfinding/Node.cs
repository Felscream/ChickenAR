﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding{
    public class Node : BasicGridTile, IHeapItem<Node>
    {
        public bool IsWalkable;

        public int GCost;
        public int HCost;
        public Node Parent;
        public float Elevation;
        public int FCost { get { return GCost + HCost; } }

        public int HeapIndex { get; set; }

        public Node(bool walk, Vector3 wp, int x, int y) : base(x, y , wp)
        {
            IsWalkable = walk;
        }

        public int CompareTo(Node other)
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


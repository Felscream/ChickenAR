using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace Pathfinding {
    [RequireComponent(typeof(PathfindingGrid))]
    public class AStarPathfinding : MonoBehaviour
    {
        private PathfindingGrid _grid;

        private void Awake()
        {
            _grid = GetComponent<PathfindingGrid>();
            if (_grid == null) {
                UnityEngine.Debug.LogError(gameObject.name + "::Component of type PathfindingGrid not found");
                enabled = false;
            }
        }

        public void FindPath(PathRequest request, Action<PathResult> callback)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Vector3[] waypoints = new Vector3[0];
            bool pathSucces = false;

            Node startNode = _grid.WorldPositionToNode(request.start);
            Node targetNode = _grid.WorldPositionToNode(request.end);

            if(startNode.IsWalkable && targetNode.IsWalkable)
            {
                MinHeap<Node> openSet = new MinHeap<Node>(_grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node current = openSet.RemoveFirst();
                    closedSet.Add(current);

                    if (current == targetNode)
                    {
                        sw.Stop();
                        UnityEngine.Debug.Log("Path found " + sw.ElapsedMilliseconds + " ms");
                        pathSucces = true;
                        break;
                    }


                    foreach (Node neighbour in current.Neighbours)
                    {
                        if (neighbour == null || !neighbour.IsWalkable || closedSet.Contains(neighbour))
                            continue;

                        int newMovementCost = current.GCost + GetDistance(current, neighbour);

                        if (newMovementCost < neighbour.GCost || !openSet.Contains(neighbour))
                        {
                            neighbour.GCost = newMovementCost;
                            neighbour.HCost = GetDistance(neighbour, targetNode);
                            neighbour.Parent = current;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
            
            if (pathSucces)
            {
                waypoints = RetracePath(startNode, targetNode);
            }

            callback(new PathResult(waypoints, pathSucces, request.callback));
        }

        private Vector3[] RetracePath(Node startNode, Node endNode) {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        private Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector3 directionOld = Vector3.zero;

            for(int i = 0; i < path.Count - 1; i++)
            {
                Vector3 directionNew = new Vector3(path[i].GridX - path[i + 1].GridX, path[i].Elevation - path[i+1].Elevation, path[i].GridY - path[i+1].GridY);
                if(directionNew != directionOld)
                {
                    waypoints.Add(path[i].WorldPosition);
                    directionOld = directionNew;
                }
            }

            return waypoints.ToArray();
        }

        int GetDistance(Node a, Node b) {
            int distX = Mathf.Abs(a.GridX - b.GridX);
            int distY = Mathf.Abs(a.GridY - b.GridY);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
    public class AStarPathfinding : MonoBehaviour
    {

        public Transform seeker;
        public Transform target;
        private PathfindingGrid _grid;

        private void Awake()
        {
            _grid = GetComponent<PathfindingGrid>();
            if (_grid == null) {
                Debug.LogError(gameObject.name + "::Component of type PathfindingGrid not found");
                enabled = false;
            }
        }

        private void Update()
        {
            FindPath(seeker.position, target.position);    
        }

        private void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            Node startNode = _grid.WorldPositionToNode(startPosition);
            Node targetNode = _grid.WorldPositionToNode(targetPosition);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node current = openSet[0];
                for (int i = 1; i < openSet.Count; ++i)
                {
                    if (openSet[i].fCost < current.fCost)
                    {
                        current = openSet[i];
                    }
                    else if (openSet[i].fCost == current.fCost && openSet[i].HCost < current.HCost) {
                        current = openSet[i];
                    }
                }

                openSet.Remove(current);
                closedSet.Add(current);

                if (current == targetNode) {
                    RetracePath(startNode, targetNode);
                    return;
                }
                    

                foreach (Node neighbour in _grid.GetNeighbours(current)) {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                        continue;

                    int newMovementCost = current.GCost + GetDistance(current, neighbour);

                    if (newMovementCost < neighbour.GCost || !openSet.Contains(neighbour)) {
                        neighbour.GCost = newMovementCost;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = current;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        void RetracePath(Node startNode, Node endNode) {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();

            _grid.path = path;
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


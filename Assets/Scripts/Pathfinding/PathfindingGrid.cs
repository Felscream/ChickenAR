using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
    public class PathfindingGrid : MonoBehaviour
    {
        public Vector2 GridWorldSize;
        public float NodeRadius;
        public LayerMask UnwalkableLayer;
        public bool OnlyDisplayPath;
        public List<Node> path;

        private Node[,] _grid;
        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY;

        public int MaxSize
        {
            get
            {
                return _gridSizeX * _gridSizeY;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

            if (OnlyDisplayPath)
            {
                if (path != null)
                {
                    Gizmos.color = Color.cyan;
                    foreach (Node n in path)
                    {
                        Gizmos.DrawCube(n.WorldPosition, (Vector3.right + Vector3.forward) * (_nodeDiameter - 0.1f) + Vector3.up * 0.2f);
                    }
                }
            }
            else if(_grid != null) {
                foreach (Node n in _grid) {
                    Gizmos.color = n.IsWalkable ? Color.white : Color.red;

                    if (path != null) {
                        if(path.Contains(n))
                            Gizmos.color = Color.cyan;
                    }

                    Gizmos.DrawCube(n.WorldPosition, (Vector3.right + Vector3.forward) * (_nodeDiameter - 0.1f) + Vector3.up * 0.2f);
                }
            }
        }

        private void Start()
        {
            _nodeDiameter = 2 * NodeRadius;
            _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
            CreateGrid();
        }

        public Node WorldPositionToNode(Vector3 worldPosition) {
            float percentX = Mathf.Clamp01((worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x);
            float percentY = Mathf.Clamp01((worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
            return _grid[x, y];
        }

        public List<Node> GetNeighbours(Node node) {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++) {
                for(int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    Vector2 coord = new Vector2(node.GridX, node.GridY) + new Vector2(x, y);
                    if(coord.x >= 0 && coord.x < _gridSizeX && coord.y >= 0 && coord.y < _gridSizeY)
                    {
                        neighbours.Add(_grid[(int)coord.x, (int)coord.y]);
                    }
                }
            }

            return neighbours;
        }

        private void CreateGrid() {
            _grid = new Node[_gridSizeX, _gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;
            for(int x = 0; x < _gridSizeX; ++x)
            {
                for (int y = 0; y < _gridSizeY; ++y) {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) + Vector3.forward * (y * _nodeDiameter + NodeRadius);
                    bool walkable = !Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableLayer);
                    _grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }
    }
}


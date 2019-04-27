using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

namespace Pathfinding {
    public class PathfindingGrid : BasicGrid<Node>
    {
        public LayerMask UnwalkableLayer;
        public int MaxSize
        {
            get
            {
                return _gridSizeX * _gridSizeY;
            }
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

            if (_grid != null && DisplayGrid)
            {
                foreach (Node n in _grid)
                {
                    Gizmos.color = n.IsWalkable ? Color.white : Color.black;
                    Gizmos.DrawCube(n.WorldPosition, (Vector3.right + Vector3.forward) * (_nodeDiameter - 0.1f) + Vector3.up * 0.2f);
                }
            }
        }

        protected override void Awake()
        {
            
        }

        public Node GetNode(int x, int y)
        {
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

        public void RefreshGrid()
        {
            if(_grid != null)
            {
                for (int x = 0; x < _gridSizeX; ++x)
                {
                    for (int y = 0; y < _gridSizeY; ++y)
                    {
                        Node n = _grid[x, y];
                        bool walkable = !Physics.CheckSphere(n.WorldPosition, NodeRadius, UnwalkableLayer);
                        _grid[x, y].IsWalkable = walkable;
                    }
                }
            }
        }

        public void UpdateNode(TerrainTile tile)
        {
            bool walkable = tile.Type != TileType.Water;
            
            if (_grid == null)
            {
                Debug.LogError("Grid not generated yet", gameObject);
            }
            Node nodeToUpdate = _grid[tile.X, tile.Y];
            nodeToUpdate.IsWalkable = walkable;
            nodeToUpdate.WorldPosition = new Vector3(nodeToUpdate.WorldPosition.x, tile.transform.localPosition.y + tile.PivotOffset.y * 2, nodeToUpdate.WorldPosition.z);
        }

        public override void CreateGrid() {
            _grid = new Node[_gridSizeX, _gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;
            for (int x = 0; x < _gridSizeX; ++x)
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


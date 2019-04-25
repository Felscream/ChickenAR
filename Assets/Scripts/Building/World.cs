using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public class World : BasicGrid<Tile>
    {
        public LayerMask ObstacleLayer;

        protected override void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

            if (_grid != null && DisplayGrid)
            {
                foreach (Tile n in _grid)
                {
                    switch (n.Type)
                    {
                        case TileType.Blocked:
                            Gizmos.color = Color.red;
                            break;
                        default:
                            Gizmos.color = Color.green;
                            break;
                    }
                    Gizmos.DrawCube(n.WorldPosition, (Vector3.right + Vector3.forward) * (_nodeDiameter - 0.1f) + Vector3.up * 0.2f);
                }
            }
        }

        protected override void CreateGrid()
        {
            _grid = new Tile[_gridSizeX, _gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;
            for (int x = 0; x < _gridSizeX; ++x)
            {
                for (int y = 0; y < _gridSizeY; ++y)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) + Vector3.forward * (y * _nodeDiameter + NodeRadius);
                    TileType t = Physics.CheckSphere(worldPoint, NodeRadius, ObstacleLayer) ? TileType.Blocked : TileType.Empty;
                    _grid[x, y] = new Tile(x, y , worldPoint, t);
                }
            }
        }
    }
}


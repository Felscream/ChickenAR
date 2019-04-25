using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicGrid<T> : MonoBehaviour where T : BasicGridTile
{
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public bool DisplayGrid;

    protected T[,] _grid;
    protected float _nodeDiameter;
    protected int _gridSizeX, _gridSizeY;


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

        if (_grid != null && DisplayGrid)
        {
            foreach (T n in _grid)
            {
                Gizmos.DrawCube(n.WorldPosition, (Vector3.right + Vector3.forward) * (_nodeDiameter - 0.1f) + Vector3.up * 0.2f);
            }
        }
    }

    protected void Awake()
    {
        _nodeDiameter = 2 * NodeRadius;
        _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    public T WorldPositionToNode(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
        return _grid[x, y];
    }

    public T GetTile(int x, int y)
    {
        return _grid[x, y];
    }

    protected abstract void CreateGrid();

}

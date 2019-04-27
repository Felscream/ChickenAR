using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;
using Pathfinding;

public class WorldGeneratorController : MonoBehaviour
{
    public TerrainGenerator _terrainGrid;
    public PathfindingGrid _pathfindingGrid;

    private void Awake()
    {
        if (_pathfindingGrid != null)
        {
            _pathfindingGrid.Initialize();
        }
        else
        {
            Debug.LogError("No reference to PathfindingGrid", gameObject);
        }

        if (_terrainGrid != null)
        {
            _terrainGrid.Initialize();
        }
        else
        {
            Debug.LogError("No reference to TerrainGenerator", gameObject);
        }
    }
}

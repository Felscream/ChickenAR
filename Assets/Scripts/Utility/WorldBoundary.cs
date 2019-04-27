using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

[RequireComponent(typeof(TerrainGenerator))]
public class WorldBoundary : MonoBehaviour
{
    private static WorldBoundary _instance;

    public static Vector3[] Boundaries { get; private set; }

    private TerrainGenerator _terrainGenerator;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }

        ComputeBoundaries();
    }

    private void ComputeBoundaries()
    {
        Boundaries = new Vector3[2];
        _terrainGenerator = GetComponent<TerrainGenerator>();
        Boundaries[0] = transform.position - Vector3.right * _terrainGenerator.Dimensions.x / 2f;
        Boundaries[1] = transform.position + Vector3.forward * _terrainGenerator.Dimensions.y / 2f;
    }
}

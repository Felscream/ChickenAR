using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class WorldBoundary : MonoBehaviour
{
    private static WorldBoundary _instance;

    public static Vector3[] Boundaries { get; private set; }

    private Terrain _terrain;

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
        _terrain = GetComponent<Terrain>();
        Boundaries[0] = transform.position;
        Boundaries[1] = transform.position + _terrain.terrainData.size;
    }
}

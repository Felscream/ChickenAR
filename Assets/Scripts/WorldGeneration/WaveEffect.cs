using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

[RequireComponent(typeof(TerrainGenerator))]
public class WaveEffect : MonoBehaviour
{

    public float Frequency = 0.5f;
    public float Amplitude = 1f;
    public float XOffset = 0.4f;
    public float YOffset = 0.4f;
    private TerrainGenerator _terraingGenerator;

    private List<TerrainTile> _waterTiles;

    private void Start()
    {
        _terraingGenerator = GetComponent<TerrainGenerator>();
        _waterTiles = _terraingGenerator.GetTilesByType(TileType.Water);
    }

    private void Update()
    {
        for(int i = 0; i < _waterTiles.Count; i++)
        {
            float y = Mathf.Sin(2f * Mathf.PI * Frequency * Time.time + 1.5f + _waterTiles[i].X * XOffset + _waterTiles[i].Y * YOffset) * Amplitude;
            _waterTiles[i].transform.localPosition = _waterTiles[i].FixedLocalPosition + Vector3.up * y;
        }
    }
}

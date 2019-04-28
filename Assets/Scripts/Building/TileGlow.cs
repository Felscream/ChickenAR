using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

public class TileGlow : MonoBehaviour
{
    public TerrainGenerator _terrain;

    [ColorUsageAttribute(false, true)] public Color InteractableTile;
    [ColorUsageAttribute(false, true)] public Color BlockedTile;

    private TerrainTile[,] _tiles;
    

    private void Start()
    {
        if(_terrain != null)
        {
            _tiles = _terrain.Grid;
            foreach(TerrainTile t in _tiles)
            {
                t.Glow += Glow;
            }
        }
    }

    private void Glow(TerrainTile tile)
    {
        float emissionOn = 1f;
        Color emission = InteractableTile;
        if (tile.IsHovered)
        {
            
            if (tile.HasFeature || tile.Elevation <= (int)TileType.Water)
            {
                emission = BlockedTile;
            }
        }
        else
        {
            emissionOn = 0f;
        }

        tile.Renderer.GetPropertyBlock(tile.MaterialProperty);

        tile.MaterialProperty.SetColor("_Emission", emission);
        tile.MaterialProperty.SetFloat("_EmissionLerp", emissionOn);

        tile.Renderer.SetPropertyBlock(tile.MaterialProperty);
    }
}

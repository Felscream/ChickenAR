using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

public class TileSelection : MonoBehaviour
{
    public delegate bool TileFiltering(TerrainTile tile);
    public event TileFiltering IsTilePlayable;
    public TerrainGenerator _terrain;

    [ColorUsageAttribute(false, true)] public Color InteractableTile;
    [ColorUsageAttribute(false, true)] public Color BlockedTile;

    public bool AreTilesSelectable = false;
    private TerrainTile _currentTile;

    private TerrainTile[,] _tiles;
    public TerrainTile SelectedTile { get { return _currentTile; } }

    private void Start()
    {
        if(_terrain != null)
        {
            _tiles = _terrain.Grid;
            foreach(TerrainTile t in _tiles)
            {
                t.OnTileHover += TileSelectionBehaviour;
            }
        }
    }

    private void Glow(TerrainTile tile)
    {
        float emissionOn = 0f;
        Color emission = BlockedTile;
        if (tile.IsHovered && AreTilesSelectable)
        {
            emissionOn = 1f;
            if (IsTilePlayable != null && IsTilePlayable(tile))
            {
                emission = InteractableTile;
            }
        }

        tile.Renderer.GetPropertyBlock(tile.MaterialProperty);

        tile.MaterialProperty.SetColor("_Emission", emission);
        tile.MaterialProperty.SetFloat("_EmissionLerp", emissionOn);

        tile.Renderer.SetPropertyBlock(tile.MaterialProperty);

        _currentTile = tile;
    }

    private void TileSelectionBehaviour(TerrainTile tile)
    {
        Glow(tile);
    }
}

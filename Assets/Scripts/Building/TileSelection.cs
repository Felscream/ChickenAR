﻿using System.Collections;
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
    public TerrainTile CurrentTile { get; private set; }

    private TerrainTile[,] _tiles;
    private TouchManager _touchManager;

    private void Start()
    {
        _touchManager = TouchManager.Instance;
        if(_terrain != null)
        {
            _tiles = _terrain.Grid;
            foreach(TerrainTile t in _tiles)
            {
                t.OnTileHover += TileSelectionBehaviour;
            }
        }
    }

    private void Update()
    {
        FetchSelectedTile();
    }

    private void FetchSelectedTile()
    {
        CurrentTile = null;
        if (_touchManager.CurrentTouchable is TerrainTile temp)
        {
            if (temp.IsAtSurface)
            {
                CurrentTile = temp;
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
    }

    private void TileSelectionBehaviour(TerrainTile tile)
    {
        Glow(tile);
    }
}

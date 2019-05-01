using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

public abstract class CardEffect : ScriptableObject
{
    public TileType[] PlayableTiles;
    public bool CanBePlayedOnFeatures = false;
    public bool OnlyPlayableOnFeatures = false;
    public abstract void Execute(TerrainTile tile);
}

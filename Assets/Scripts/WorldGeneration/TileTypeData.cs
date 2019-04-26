using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerator
{
    public enum TileType
    {
        Water,
        Mud,
        Grass,
        Rock,
        Snow,
        Ice,
        GrassSnow,
        CutGrass,
        GrassRock
    }

    [System.Serializable]
    public class TileTypeData
    {
        public TileType Type;
        public TerrainTile Tile;
    }
}


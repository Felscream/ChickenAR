using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public enum TileType
    {
        Empty,
        Blocked,
        Floor
    }

    public class Tile : BasicGridTile
    {
        public TileType Type = TileType.Empty;

        private LooseObject _looseObject;
        private InstalledObject _installedObject;

        public Tile(int x, int y, Vector3 wp, TileType type = TileType.Empty) : base(x, y, wp)
        {
            Type = type;
        }
    }
}


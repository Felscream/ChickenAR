using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerator
{
    public struct TileHash
    {
        public float a, b;

        public static TileHash Create()
        {
            TileHash hash;
            hash.a = Random.value;
            hash.b = Random.value;
            return hash;
        }
    }

    public class TerrainTile : MonoBehaviour
    {
        public Vector3 PivotOffset = new Vector3(-0.5f, 0.5f, 0.5f);
        [Range(0f,1f)] public float FeatureProbability;
        public TerrainTile[] Neighbours = new TerrainTile[8];
        public TileType Type;
        private int _x, _y;
        private float _elevation = 0f;

        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public int Distance { get; set; }
        public Vector3 FixedLocalPosition { get; set; }
        public bool HasFeature { get; set; }
        public Vector3 TileTopCenter
        {
            get
            {
                return transform.localPosition + Vector3.up * PivotOffset.y + PivotOffset;
            }
        }
        public float Elevation
        {
            get { return _elevation; }
            set
            {
                _elevation = Mathf.Min(value, WorldConstants.MaxElevation);
                transform.localPosition = new Vector3(transform.localPosition.x, _elevation / 2f - PivotOffset.y, transform.localPosition.z);
                FixedLocalPosition = transform.localPosition;
            }
        }

        public int SearchHeuristic { get; set; }
        public int SearchPriority
        {
            get
            {
                return Distance + SearchHeuristic;
            }
        }

        public int SearchPhase { get; set; }

        public TerrainTile NextWithSamePriority { get; set; }

        public void SetCoordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public TerrainTile GetNeighbour(WorldConstants.NeighbourDirection direction)
        {
            return Neighbours[(int)direction];
        }

        public void SetNeighbour(WorldConstants.NeighbourDirection direction, TerrainTile tile)
        {
            Neighbours[(int)direction] = tile;
            tile.Neighbours[(int)direction.Opposite()] = this;
        }

        public int DistanceTo(TerrainTile t)
        {
            return Mathf.RoundToInt(Vector2.Distance(new Vector2(X, Y), new Vector2(t.X, t.Y)));
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace WorldGenerator
{
    public class TerrainGenerator : MonoBehaviour
    {
        public Vector2 Dimensions;
        public PathfindingGrid PathfindingGrid;

        public float TileRadius = 0.5f;
        public TileTypeData[] TilePrefabs;
        [Range(0, 1)] public float MudToGrassProbability = 0.4f;
        [Range(0f, 0.5f)] public float JitterProbability = 0.25f;
        [Range(400, 1400)] public int chunkSizeMin = 1100;
        [Range(400, 1400)] public int chunkSizeMax = 1400;
        [Range(5, 95)] public int landPercentage = 50;
        [Range(0f, 1f)] public float highRiseProbability = 0.25f;
        [Range(0f, 0.4f)] public float SinkProbability = 0.25f;
        private TerrainTile[,] _grid;
        private float _tileDiameter;
        private int _gridSizeX, _gridSizeY;
        private TilePriorityQueue _searchFrontier;
        private int _searchFrontierPhase;
        private bool _worldGenerated = false;
        private void Awake()
        {
            _tileDiameter = 2 * TileRadius;
            _gridSizeX = Mathf.RoundToInt(Dimensions.x / _tileDiameter);
            _gridSizeY = Mathf.RoundToInt(Dimensions.y / _tileDiameter);
            CreateGrid();

            if(_searchFrontier == null)
            {
                _searchFrontier = new TilePriorityQueue();
            }

            CreateLand();

            SetTerrainType();
            for(int i = 0; i < _gridSizeX; i++)
            {
                for(int j = 0; j < _gridSizeY; j++)
                {
                    _grid[i, j].SearchPhase = 0;
                }
            }
        }

        public TerrainTile GetTile(int x, int y)
        {
            return _grid[x, y];
        }

        public TerrainTile WorldPositionToNode(Vector3 worldPosition)
        {
            float percentX = Mathf.Clamp01((worldPosition.x + _gridSizeX / 2) / Dimensions.x);
            float percentY = Mathf.Clamp01((worldPosition.z + _gridSizeY / 2) / Dimensions.y);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
            return _grid[x, y];
        }

        protected virtual void CreateGrid()
        {
            _grid = new TerrainTile[_gridSizeX, _gridSizeY];
            Vector3 bottomLeft = transform.position - Vector3.right * Dimensions.x / 2 - Vector3.forward * Dimensions.y / 2;
            for (int y = 0; y < _gridSizeY; y++)
            {
                for (int x = 0; x < _gridSizeX; x++)
                {
                    CreateCell(x, y, bottomLeft);
                }
            }
            _worldGenerated = true;
        }

        private void CreateLand()
        {
            int landBudget = Mathf.RoundToInt(_gridSizeX * _gridSizeY * landPercentage * 0.01f);
            while(landBudget > 0)
            {
                int chunkSize = UnityEngine.Random.Range(chunkSizeMin, chunkSizeMax);
                if(UnityEngine.Random.value < SinkProbability)
                {
                    landBudget = SinkTerrain(chunkSize, landBudget);
                }
                else
                {
                    landBudget = RaiseTerrain(chunkSize, landBudget);
                }
                
            }
        }

        private void CreateCell(int x, int y, Vector3 reference)
        {
            TerrainTile ti = GetPrefabByType(TileType.Water);
            TerrainTile tile = _grid[x, y] = Instantiate<TerrainTile>(ti);
            Vector3 position = reference + Vector3.right * (x * _tileDiameter + TileRadius) + Vector3.forward * (y * _tileDiameter + TileRadius);
            tile.transform.SetParent(transform, false);
            tile.transform.localPosition = position - tile.PivotOffset;
            tile.transform.localScale *= _tileDiameter;
            tile.SetCoordinates(x, y);
            tile.Neighbours = new TerrainTile[8];

            ComputeTileNeighbours(tile, x, y);
            PathfindingGrid.UpdateNode(tile);
        }

        private void ComputeTileNeighbours(TerrainTile tile, int x = 0, int y = 0)
        {
            if (!_worldGenerated)
            {
                if (x > 0)
                {
                    tile.SetNeighbour(WorldConstants.NeighbourDirection.Left, _grid[x - 1, y]);
                }
                if (y > 0)
                {
                    tile.SetNeighbour(WorldConstants.NeighbourDirection.Bottom, _grid[x, y - 1]);
                    if (x + 1 < _gridSizeX)
                        tile.SetNeighbour(WorldConstants.NeighbourDirection.BottomRight, _grid[x + 1, y - 1]);

                    if (x > 0)
                    {
                        tile.SetNeighbour(WorldConstants.NeighbourDirection.BottomLeft, _grid[x - 1, y - 1]);
                    }
                }
            }
            else
            {
                
                if (tile.X > 0)
                {
                    tile.SetNeighbour(WorldConstants.NeighbourDirection.Left, _grid[tile.X - 1, tile.Y]);
                    if (tile.X + 1 < _gridSizeX)
                    {
                        tile.SetNeighbour(WorldConstants.NeighbourDirection.Right, _grid[tile.X + 1, tile.Y]);
                    }
                }

                if (tile.Y > 0)
                {
                    tile.SetNeighbour(WorldConstants.NeighbourDirection.Bottom, _grid[tile.X, tile.Y - 1]);
                    if (tile.X + 1 < _gridSizeX)
                        tile.SetNeighbour(WorldConstants.NeighbourDirection.BottomRight, _grid[tile.X + 1, tile.Y - 1]);

                    if (tile.X > 0)
                    {
                        tile.SetNeighbour(WorldConstants.NeighbourDirection.BottomLeft, _grid[tile.X - 1, tile.Y - 1]);
                    }
                }

                if (tile.Y + 1 < _gridSizeY)
                {
                    tile.SetNeighbour(WorldConstants.NeighbourDirection.Top, _grid[tile.X, tile.Y + 1]);
                    if(tile.X + 1 > _gridSizeX)
                    {
                        tile.SetNeighbour(WorldConstants.NeighbourDirection.TopRight, _grid[tile.X+1, tile.Y + 1]);
                    }

                    if (tile.X > 0)
                    {
                        tile.SetNeighbour(WorldConstants.NeighbourDirection.TopLeft, _grid[tile.X - 1, tile.Y + 1]);
                    }
                }
            }
        }

        private TerrainTile ChangeTileType(TerrainTile original, TileType newType)
        {
            TerrainTile tt = GetPrefabByType(newType);
            TerrainTile newT = Instantiate(tt);
            newT.transform.SetParent(transform, false);
            newT.transform.localPosition = original.transform.localPosition;
            newT.transform.localScale = original.transform.localScale;
            newT.SetCoordinates(original.X, original.Y);
            newT.Elevation = original.Elevation;
            newT.Distance = original.Distance;
            newT.Neighbours = original.Neighbours;
            newT.SearchHeuristic = original.SearchHeuristic;
            newT.SearchPhase = original.SearchPhase;
            newT.NextWithSamePriority = original.NextWithSamePriority;
            _grid[original.X, original.Y] = newT;
            DestroyImmediate(original.gameObject);
            ComputeTileNeighbours(newT);
            PathfindingGrid.UpdateNode(newT);
            return newT;
        }

        private int RaiseTerrain(int chunkSize, int budget)
        {
            _searchFrontierPhase++;
            TerrainTile firstTile = GetRandomTile();
            firstTile.SearchPhase = _searchFrontierPhase;
            firstTile.Distance = 0;
            firstTile.SearchHeuristic = 0;
            _searchFrontier.Enqueue(firstTile);

            int rise = UnityEngine.Random.value < highRiseProbability ? 2 : 1;
            int size = 0;
            while(size < chunkSize && _searchFrontier.Count > 0)
            {
                TerrainTile current = _searchFrontier.Dequeue();
                float originalElevation = current.Elevation;
                current.Elevation = originalElevation + rise;
                if (originalElevation < (int)TileType.Mud &&
                    current.Elevation >= (int)TileType.Mud && --budget == 0)
                {
                    break;
                }
                
                size++;

                for(WorldConstants.NeighbourDirection d = WorldConstants.NeighbourDirection.TopLeft; d <= WorldConstants.NeighbourDirection.Right; d++)
                {
                    TerrainTile neighbour = current.GetNeighbour(d);
                    if(neighbour && neighbour.SearchPhase < _searchFrontierPhase)
                    {
                        neighbour.SearchPhase = _searchFrontierPhase;
                        neighbour.Distance = neighbour.DistanceTo(firstTile);
                        neighbour.SearchHeuristic = UnityEngine.Random.value < JitterProbability ? 1 : 0;
                        _searchFrontier.Enqueue(neighbour);
                    }
                }
            }
            _searchFrontier.Clear();
            return budget;
        }

        private int SinkTerrain(int chunkSize, int budget)
        {
            _searchFrontierPhase++;
            TerrainTile firstTile = GetRandomTile();
            firstTile.SearchPhase = _searchFrontierPhase;
            firstTile.Distance = 0;
            firstTile.SearchHeuristic = 0;
            _searchFrontier.Enqueue(firstTile);

            int sink = UnityEngine.Random.value < highRiseProbability ? 2 : 1;
            int size = 0;
            while (size < chunkSize && _searchFrontier.Count > 0)
            {
                TerrainTile current = _searchFrontier.Dequeue();
                float originalElevation = current.Elevation;
                current.Elevation = originalElevation - sink;
                if (originalElevation >= (int)TileType.Mud &&
                    current.Elevation < (int)TileType.Mud)
                {
                    budget++;
                }

                size++;

                for (WorldConstants.NeighbourDirection d = WorldConstants.NeighbourDirection.TopLeft; d <= WorldConstants.NeighbourDirection.Right; d++)
                {
                    TerrainTile neighbour = current.GetNeighbour(d);
                    if (neighbour && neighbour.SearchPhase < _searchFrontierPhase)
                    {
                        neighbour.SearchPhase = _searchFrontierPhase;
                        neighbour.Distance = neighbour.DistanceTo(firstTile);
                        neighbour.SearchHeuristic = UnityEngine.Random.value < JitterProbability ? 1 : 0;
                        _searchFrontier.Enqueue(neighbour);
                    }
                }
            }
            _searchFrontier.Clear();
            return budget;
        }

        private void SetTerrainType()
        {
            for(int i = 0; i < _gridSizeX; i++)
            {
                for(int j = 0; j < _gridSizeY; j++)
                {
                    TerrainTile tile = GetTile(i, j);
                    if(tile.Elevation > (float)TileType.Water)
                    {
                        ChangeTileType(tile, (TileType)tile.Elevation);
                    }
                }
            }
        }

        private void FillSpace()
        {
            for(int i = 0; i < _gridSizeX; i++)
            {
                for(int j = 0; j < _gridSizeY; j++)
                {
                    TerrainTile tile = _grid[i, j];

                }
            }
        }

        private TerrainTile GetRandomTile()
        {
            return GetTile(UnityEngine.Random.Range(0, _gridSizeX), UnityEngine.Random.Range(0, _gridSizeY));
        }

        private TerrainTile GetPrefabByType(TileType t)
        {
            return Array.Find(TilePrefabs, f => f.Type == t).Tile;
        }
    }
}


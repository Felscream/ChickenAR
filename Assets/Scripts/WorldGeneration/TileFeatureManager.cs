using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

public class TileFeatureManager : MonoBehaviour
{

    public Transform FeaturePrefab;
    public Texture2D NoiseTexture;

    public const int HashGridSize = 256;
    public const float HashGridScale = 0.25f;

    private static TileHash[] _hashGrid;

    private Transform _container;

    public void InitializeHashGrid(int seed)
    {
        _hashGrid = new TileHash[HashGridSize * HashGridSize];
        Random.State currentState = Random.state;
        Random.InitState(seed);
        for(int i = 0; i < _hashGrid.Length; i++)
        {
            _hashGrid[i] = TileHash.Create();
        }
        Random.state = currentState;
    }

    public void Clear() {
        if (_container)
        {
            DestroyImmediate(_container.gameObject);
        }
        _container = new GameObject("Features Container").transform;
        _container.SetParent(transform);
    }
    
    public void Apply() { }

    public void AddFeature(TerrainTile tile, Vector3 position) {
        TileHash hash = SampleHashGrid(position);
        if(hash.a > 0.5 * tile.FeatureProbability)
        {
            return;
        }
        Transform instance = Instantiate(FeaturePrefab);
        instance.localPosition = position;
        instance.localRotation = Quaternion.Euler(0f, 360f * hash.b, 0f);
        instance.SetParent(_container, false);
        tile.HasFeature = true;
    }

    private TileHash SampleHashGrid(Vector3 position)
    {
        int x = (int)(position.x * HashGridScale) % HashGridSize;
        if(x < 0)
        {
            x += HashGridSize;
        }

        int z = (int)(position.z * HashGridScale)% HashGridSize;
        if (z < 0)
        {
            z += HashGridSize;
        }

        return _hashGrid[x + z * HashGridSize];
    }
}

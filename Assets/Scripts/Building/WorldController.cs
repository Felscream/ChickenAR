using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Building;

public class WorldController : MonoBehaviour
{
    public int Width;
    public int Height;
    public float TileLength; 
    private World _buildingWorld;

    private void Awake()
    {
        _buildingWorld = GetComponent<World>();
    }
}

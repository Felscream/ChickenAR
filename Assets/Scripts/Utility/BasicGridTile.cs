using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGridTile
{
    protected Vector3 _worldPosition;
    protected int _gridX;
    protected int _gridY;

    public Vector3 WorldPosition {
        get { return _worldPosition; }
        set { _worldPosition = value; }
    }
    public int GridX { get { return _gridX; } }
    public int GridY { get { return _gridY; } }

    public BasicGridTile(int x, int y, Vector3 wp)
    {
        _gridX = x;
        _gridY = y;
        _worldPosition = wp;
    }
}

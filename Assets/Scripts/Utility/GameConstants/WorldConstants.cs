using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldConstants
{
    public static int MaxElevation = 4;
    public static int MinimumElevation = 2;
    public static float ElevationStep = 1f;
}

public enum NeighbourDirection
{
    TopLeft,
    Top,
    TopRight,
    Left,
    BottomRight,
    Bottom,
    BottomLeft,
    Right
}

public static class NeighbourDirectionExtensions
{

    public static NeighbourDirection Opposite(this NeighbourDirection direction)
    {
        return (int)direction < 4 ? (direction + 4) : (direction - 4);
    }

}

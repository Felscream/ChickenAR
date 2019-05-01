using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGenerator;

[CreateAssetMenu(fileName = "CardSpawn", menuName = "CardEffect/CardSpawn")]
public class CardSpawn : CardEffect
{
    public Transform ObjectToSpawn;

    public override void Execute(TerrainTile tile)
    {
        Transform spawned = Instantiate(ObjectToSpawn, null);
        spawned.position = tile.TileTopCenter;
    }
}

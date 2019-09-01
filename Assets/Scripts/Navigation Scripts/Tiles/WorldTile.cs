using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile
{
    public Vector3Int LocalPlace { get; set; }

    public Vector3 WorldLocation { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap TilemapMember { get; set; }

    public string Name { get; set; }

    public int Cost { get; set; }

    public int TileNavigationState { get; set; } = 0;
    public enum TileType
    {
        NonWalkable  = 0x00, // Is this wrong ?
        Walkable =     0x01,
        Navigateable = 0x02,
    }

    public bool Walkable => (TileNavigationState & (int)TileType.Walkable) == (int)TileType.Walkable;
}

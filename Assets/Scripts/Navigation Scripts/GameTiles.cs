using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A Static Class to hold data about our TileMap
/// </summary>
public class GameTiles : MonoBehaviour
{
    public static GameTiles instance;
    public Tilemap Tilemap;

    public bool ShowDebug;
    public GameObject DebugObject;
    public GameObject DebugObject2;

    public Dictionary<Vector3, WorldTile> tiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        GetWorldTiles();
    }

    // Use this for initialization
    private void GetWorldTiles()
    {
        tiles = new Dictionary<Vector3, WorldTile>();
        foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            if (!Tilemap.HasTile(localPlace)) continue;
            var tile = new WorldTile
            {
                LocalPlace = localPlace,
                WorldLocation = Tilemap.CellToWorld(localPlace),
                TileBase = Tilemap.GetTile(localPlace),
                TilemapMember = Tilemap,
                Name = localPlace.x + "," + localPlace.y,
                Cost = 1 // TODO: Change this with the proper cost from ruletile
            };


            tiles.Add(tile.WorldLocation, tile);
        }

        foreach (var tile in tiles.Keys)
        {
            UpdateTile(tiles[tile]);
        }

        if (!ShowDebug)
        {
            return;
        }
        foreach (var tile in tiles.Keys)
        {
            var t = (tiles[tile]);

            if ((t.TileNavigationState & (int)WorldTile.TileType.Walkable) == (int)WorldTile.TileType.Walkable)
            {
                t.TilemapMember.SetColor(t.LocalPlace, Color.green);
                //Debug.DrawLine(Tilemap.GetCellCenterWorld(t.LocalPlace), Vector3.zero, Color.green, 5);
                Instantiate(DebugObject2, Tilemap.GetCellCenterWorld(t.LocalPlace), Quaternion.identity);
                //Debug.DrawLine(t.WorldLocation, Vector3.zero, Color.green, 5);
            }
            else if ((t.TileNavigationState & (int)WorldTile.TileType.Navigateable) == (int)WorldTile.TileType.Navigateable)
            {
                t.TilemapMember.SetColor(t.LocalPlace, Color.yellow);
                Instantiate(DebugObject, Tilemap.GetCellCenterWorld(t.LocalPlace), Quaternion.identity);
                Debug.DrawLine(Tilemap.GetCellCenterWorld(t.LocalPlace), Vector3.zero, Color.yellow, 10);
            }
        }
    }
    public void UpdateTile(Vector3 pos)
    {
        if (tiles.TryGetValue(pos, out var x))
        {
            UpdateTile(x);
        }
    }

    public WorldTile GetRandomNavigateableWorldTile()
    {
        return tiles.Values.Where(x => x.Walkable).OrderBy(x => Random.Range(0, 500)).First();
    }

    public void UpdateTile(WorldTile tile)
    {
        if (!(tile.TileBase is WalkableSurfaceTile))
        {
            return;
        }
        var setState = false;
        //Check if really navigateable.
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                var location = new Vector3(tile.WorldLocation.x + i, tile.WorldLocation.y + j);
                if (tiles.TryGetValue(location, out var worldTile) && !(worldTile.TileBase is WalkableSurfaceTile))
                {
                    setState = true;
                    //we have a tile and it is not another walkable surface tile.
                    //TODO: this might be too simple
                    if (i != 0 && j != 0)
                    {
                        //We are on the diagonal
                        tile.TileNavigationState |= (int)WorldTile.TileType.Navigateable;
                    }
                    else
                    {
                        tile.TileNavigationState |= (int)WorldTile.TileType.Walkable;
                    }
                }
            }
        }
        if (!setState)
        {
            tile.TileNavigationState ^= tile.TileNavigationState;
        }

        if ((tile.TileNavigationState & (int)WorldTile.TileType.Walkable) == (int)WorldTile.TileType.Walkable)
        {
            tile.TilemapMember.SetColor(tile.LocalPlace, Color.green);
            //Debug.DrawLine(Tilemap.GetCellCenterWorld(t.LocalPlace), Vector3.zero, Color.green, 5);
            Instantiate(DebugObject2, Tilemap.GetCellCenterWorld(tile.LocalPlace), Quaternion.identity);
            //Debug.DrawLine(t.WorldLocation, Vector3.zero, Color.green, 5);
        }
        else if ((tile.TileNavigationState & (int)WorldTile.TileType.Navigateable) == (int)WorldTile.TileType.Navigateable)
        {
            tile.TilemapMember.SetColor(tile.LocalPlace, Color.yellow);
            Instantiate(DebugObject, Tilemap.GetCellCenterWorld(tile.LocalPlace), Quaternion.identity);
            Debug.DrawLine(Tilemap.GetCellCenterWorld(tile.LocalPlace), Vector3.zero, Color.yellow, 10);
        }
    }
}
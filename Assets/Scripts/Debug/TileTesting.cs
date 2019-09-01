using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTesting : MonoBehaviour
{
    public Tile TileToSet;
    	private void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

			var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles

			if (tiles.TryGetValue(worldPoint, out var _tile)) 
			{
				print("Tile " + _tile.Name + " costs: " + _tile.TileNavigationState);
                //_tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                //_tile.TilemapMember.SetColor(_tile.LocalPlace, Color.green);
                GameTiles.instance.SetNewTile(_tile.LocalPlace, TileToSet);
			}
		}
	}
}

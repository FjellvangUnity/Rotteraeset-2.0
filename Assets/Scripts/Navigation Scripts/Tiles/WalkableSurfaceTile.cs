using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkableSurfaceTile : Tile
{

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        //GameTiles
        this.colliderType = ColliderType.None;
        if (EditorApplication.isPlaying)
        {
            GameTiles.instance.UpdateTile(position);
        }
    }
    [MenuItem("Assets/Create/Tiles/WalkableSurfaceTile ")]
    public static void CreateWalkableSurfaceTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Walkable Tile", "New Walkable Tile", "asset", "SaveWalkableTile", "Assets");
        if (path == null)
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WalkableSurfaceTile> (), path);
    }

}

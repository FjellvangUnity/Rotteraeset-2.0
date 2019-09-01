using Assets.Scripts.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarNPC))]
public class WallWalkerMovement : MonoBehaviour
{

    AStarNPC PathfindingScript;
    public float speed = 1f;
    bool found = false;
    Vector3 nextPosition;
    private void Awake()
    {
        PathfindingScript = GetComponent<AStarNPC>();
    }

    private void Start()
    {
        found = PathfindingScript.ComputeAStar(
            GameTiles.instance.Tilemap.WorldToCell(transform.position), 
            GameTiles.instance.GetRandomNavigateableWorldTile().LocalPlace);
        nextPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (PathfindingScript.Path.Count <= 0 || !found)
        {
            found = PathfindingScript.ComputeAStar(
                GameTiles.instance.Tilemap.WorldToCell(transform.position), 
                GameTiles.instance.GetRandomNavigateableWorldTile().LocalPlace);
        }
        if (PathfindingScript.Path.Count > 0 && Vector3.Distance(transform.position, nextPosition) <= .5f)
        {
            nextPosition = GameTiles.instance.Tilemap.GetCellCenterWorld(PathfindingScript.Path.Pop());
        }
        var relative = (nextPosition - transform.position).normalized;
        transform.Translate(relative * speed * Time.deltaTime);
    }
}

using Assets.Scripts.Data;
using Assets.Scripts.Navigation_Scripts.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Pathfinding
{
	public class AStarNPC : MonoBehaviour
	{
        Tilemap Map;
        public Vector3 Goal;
        [SerializeField]
		int Width = 200;
        [SerializeField]
		int Height = 200;
		public Stack<Vector3Int> Path = new Stack<Vector3Int>();


		private void Awake()
		{
            Map = GameTiles.instance.Tilemap;
		}

		public IEnumerable<CoordWithWeight> Neightbors(Vector3Int current)
		{
            yield return new CoordWithWeight(new Vector3Int(current.x + 1, current.y, 0),1);
            yield return new CoordWithWeight(new Vector3Int(current.x, current.y + 1, 0),1);
            yield return new CoordWithWeight(new Vector3Int(current.x - 1, current.y, 0),1);
            yield return new CoordWithWeight(new Vector3Int(current.x, current.y - 1, 0),1);
		}
		private double Heuistic(Vector3Int a, Vector3Int b)
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
		}
		public bool ComputeAStar(Vector3Int start, Vector3Int goal)
		{
			var frontier = new PriorityQueue<CoordWithWeight>();
			var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
			var costSoFar = new Dictionary<Vector3Int, double>();
            var goalFound = false;
            costSoFar[start] = 0;
			frontier.Enqueue(new CoordWithWeight(start, 0));
			while (frontier.Count > 0 && !goalFound)
			{
				var current = frontier.Dequeue();

				foreach (var point in Neightbors(current.Coord))
				{
                    if (Mathf.Abs(point.Coord.x) > Width || Mathf.Abs(point.Coord.y) > Height)
                    {
                        //OUT OF MAX BOUNDS
                        return false;
                    }
					var newCost = costSoFar[current.Coord] + point.Weight;
                    //Debug.DrawLine(Map.GetCellCenterWorld(current.Coord), Map.GetCellCenterWorld(point.Coord), Color.red, 2);
                    //var tile = GameTiles.instance.tiles[point.Coord];// Map.GetTile(point.Coord);
                    if (!GameTiles.instance.tiles.TryGetValue(point.Coord, out var x) || (x != null && x.TileNavigationState == 0))
                    {
                        //We have treaded where we cannot!
                        //TODO: IMPROVE THIS.
                        continue;
                    }
					if (point.Coord == goal)
					{
						goalFound = true;
						cameFrom[point.Coord] = current.Coord;
						break;
					}

					if (!costSoFar.ContainsKey(point.Coord) || 
						newCost < costSoFar[point.Coord])
					{
						costSoFar[point.Coord] = newCost;

						var priority = newCost + Heuistic(goal, point.Coord);
						frontier.Enqueue(new CoordWithWeight(point.Coord, priority));

						cameFrom[point.Coord] = current.Coord;
				    }
				}
			}
            if (!goalFound)
            {
                return false;
            }

			var next = goal;

			while (next != start)
			{
				Path.Push(next);
				next = cameFrom[next];
			}
			Path.Push(start);
			return true;
		}
	}
}

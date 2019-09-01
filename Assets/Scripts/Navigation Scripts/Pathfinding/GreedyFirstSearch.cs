using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Pathfinding
{
	public class GreedyFirstSearch : MonoBehaviour
	{
		public Tilemap Map;
		public int Width = 200;
		public int Height = 200;
		Stack<Vector3Int> Path = new Stack<Vector3Int>();

		private void Start()
		{
			//Debug.Log(GoalTile.AWOKE);
		}
		private void Update()
		{
			//Debug.Log(GoalTile.Count);
			//if (GoalTile.Count > 0 && Path.Count <= 0)
			//{
			//	ComputeGreedy(Map.WorldToCell(transform.position), Map.WorldToCell(GoalTile.tiles[0]));
			//}
			if (Path.Count > 0)
			{
				var current = Path.Pop();
				while (Path.Count > 0)
				{
					var next = Path.Pop();
					Debug.DrawLine(Map.GetCellCenterWorld(current), Map.GetCellCenterWorld(next), Color.blue, 1);
					current = next;
				}
			}
		}

		public IEnumerable<Vector3Int> Neightbors(Vector3Int current)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					yield return new Vector3Int(current.x + i, current.y + j, 0);
				}
			}
		}
		private int Heuistic(Vector3Int a, Vector3Int b)
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
		}
		class CoordWithWeight : IComparable<CoordWithWeight>
		{
			public Vector3Int Coord;
			int Weight;
			public CoordWithWeight(Vector3Int coord, int weight)
			{
				Coord = coord;
				Weight = weight;
			}
			public int CompareTo(CoordWithWeight other)
			{
				return Weight.CompareTo(other.Weight);
			}
        }
		public bool ComputeGreedy(Vector3Int start, Vector3Int goal)
		{
			var frontier = new PriorityQueue<CoordWithWeight>();
            //var visisted = new Dictionary<Vector3Int, bool>();
            var visisted = new HashSet<Vector3Int>();
			var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
			var goalFound = false;
			frontier.Enqueue(new CoordWithWeight(start, 0));
			while (frontier.Count > 0 && !goalFound)
			{
				var current = frontier.Dequeue();

                visisted.Add(current.Coord);
				//visisted[current.Coord] = true;

				foreach (var point in Neightbors(current.Coord))
				{
					Debug.DrawLine(Map.GetCellCenterWorld(current.Coord), Map.GetCellCenterWorld(point), Color.red, 2);
					var priority = Heuistic(goal, point);
					var tile = Map.GetTile(point);
					if (point == goal)
					{
						//goal = point;
						goalFound = true;
						cameFrom[point] = current.Coord;
						break;
					}

					if (visisted.Contains(point) || (tile != null && tile.GetType() == typeof(Tile)) || point.x > Width || point.y > Height)
					{
						continue;
					}

					frontier.Enqueue(new CoordWithWeight(point, priority));
                    visisted.Add(point);
					//visisted[point] = true;

					cameFrom[point] = current.Coord;
				}
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

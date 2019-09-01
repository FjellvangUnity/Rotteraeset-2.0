using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Navigation_Scripts.Pathfinding
{
    public class CoordWithWeight : IComparable<CoordWithWeight>
    {
        public Vector3Int Coord;
        public double Weight;
        public CoordWithWeight(Vector3Int coord, double weight)
        {
            Coord = coord;
            Weight = weight;
        }
        public int CompareTo(CoordWithWeight other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }
}

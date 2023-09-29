using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration
{
    public static class Direction2D
    {
        public static readonly Vector2Int NORTH = new Vector2Int(0, 1);
        public static readonly Vector2Int SOUTH = new Vector2Int(0, -1);
        public static readonly Vector2Int EAST = new Vector2Int(1, 0);
        public static readonly Vector2Int WEST = new Vector2Int(-1, 0);
        public static readonly Vector2Int[] CARDINAL = new Vector2Int[] { NORTH, SOUTH, EAST, WEST };

        public static Vector2Int GetRandomCardinalDirection()
        {
            return CARDINAL[Random.Range(0, CARDINAL.Length)];
        }
    }
}

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

        public static Vector2Int[] GetPerpendicular(Vector2Int direction)
        {
            if (direction == NORTH || direction == SOUTH)
                return new Vector2Int[] { EAST, WEST };

            if (direction == EAST || direction == WEST)
                return new Vector2Int[] { NORTH, SOUTH };

            return null;
        }

        public static Vector2Int? GetRandomCardinalDirection(IEnumerable<Vector2Int> except)
        {
            List<Vector2Int> directions = new(CARDINAL);
            foreach (Vector2Int direction in except)
                directions.Remove(direction);

            if (directions.Count == 0)
                return null;

            return directions[Random.Range(0, directions.Count)];
        }

    }
}

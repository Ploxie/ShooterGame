using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{

    [Serializable]
    public class Tile
    {
        public static class Wall // Venne var jag ska lägga denna skiten
        {
            public static readonly int NORTH = 0x1;
            public static readonly int SOUTH = 0x2;
            public static readonly int EAST = 0x4;
            public static readonly int WEST = 0x8;
            public static readonly int NORTH_DOOR = 0x10;
            public static readonly int SOUTH_DOOR = 0x20;
            public static readonly int EAST_DOOR = 0x40;
            public static readonly int WEST_DOOR = 0x80;

            public static int FromDirection(Vector2Int direction)
            {
                if (direction.x == 0 && direction.y == 1)
                    return NORTH;
                if (direction.x == 0 && direction.y == -1)
                    return SOUTH;
                if (direction.x == 1 && direction.y == 0)
                    return EAST;
                if (direction.x == -1 && direction.y == 0)
                    return WEST;

                return 0;
            }
        }

        public static readonly float TILE_SIZE = 2.5f;


        [SerializeField] public Vector2Int Position;

        private bool isCorridor;
        private bool isModule;
        private int walls;

        public Tile(Vector2Int position)
        {
            Position = position;
        }

        public Tile(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }

        public bool IsCorridor
        {
            get { return isCorridor; }
            set { isCorridor = value; }
        }

        public bool IsModule
        {
            get { return isModule; }
            set { isModule = value; }
        }

        public int Walls
        {
            get { return walls; }
            set { walls = value; }
        }

        public static implicit operator Vector2Int(Tile tile) => tile.Position;
        public static explicit operator Tile(Vector2Int position) => new Tile(position);
        public static implicit operator Vector3(Tile tile) => new Vector3(tile.Position.x, 0, tile.Position.y);

        public int WallCount
        {
            get
            {
                return ((Walls & Wall.WEST) >> 0x3) + ((Walls & Wall.EAST) >> 0x2) + ((Walls & Wall.SOUTH) >> 0x1) + (Walls & Wall.NORTH);
            }
        }

        public bool HasWall(int mask)
        {
            return ((Walls & mask) == mask);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Tile))
                return ((Tile)obj).Position == Position;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration
{
    public class Room
    {
        public RoomModule Module;
        public Vector2Int Position;
        public Room Parent;

        public BoundsInt GetBounds(int offset)
        {
            return new BoundsInt(
                new Vector3Int((int)Module.Bounds.center.x + Position.x - (int)(Module.Bounds.size.x * 0.5f) - offset, (int)Module.Bounds.center.y, (int)Module.Bounds.center.z + Position.y - (int)(Module.Bounds.size.z * 0.5f) - offset),
                new Vector3Int(Module.Bounds.size.x + (offset * 2), Module.Bounds.size.y, Module.Bounds.size.z + (offset * 2))
            );
        }

        public Vector2Int GetRandomTile()
        {
            Tile tile = Module.Tiles.ElementAt(Random.Range(0, Module.Tiles.Count));
            return new Vector2Int(Position.x + tile.Position.x, Position.y + tile.Position.y);
        }
    }
}

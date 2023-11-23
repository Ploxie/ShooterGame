using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    [ExecuteAlways]
    public class RoomModule : MonoBehaviour
    {

        public HashSet<Tile> Tiles
        {
            get;
            private set;
        }

        public BoundsInt Bounds
        {
            get;
            private set;
        }

        public Vector2Int Size
        {
            get;
            private set;
        }

        private void Update()
        {
            //CalculateTiles();
        }

        public HashSet<Tile> CalculateTiles()
        {
            Tiles = new HashSet<Tile>();
            var floors = GetComponentsInChildren<RoomFloor>();
            foreach (var floor in floors)
            {
                Tiles.UnionWith(floor.CalculateTiles());
            }
            return Tiles;
        }

        public BoundsInt CalculateBounds()
        {
            Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
            Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);

            var tiles = CalculateTiles();

            foreach (Tile tile in tiles)
            {
                if (tile.Position.x <= min.x)
                    min.x = tile.Position.x;
                if (tile.Position.x >= max.x)
                    max.x = tile.Position.x;
                if (tile.Position.y <= min.y)
                    min.y = tile.Position.y;
                if (tile.Position.y >= max.y)
                    max.y = tile.Position.y;
            }

            Bounds = new BoundsInt(min.x, 0, min.y, max.x - min.x, 0, max.y - min.y);
            Size = new Vector2Int(Bounds.size.x, Bounds.size.z);
            return Bounds;
        }

        private void OnDrawGizmos()
        {
            if (Tiles == null)
                return;

            Gizmos.color = Color.green;

            foreach(var tile in Tiles)
            {
                Gizmos.DrawWireCube(new Vector3(tile.Position.x + 0.5f, 0.0f, tile.Position.y + 0.5f) * Tile.TILE_SIZE, new Vector3(Tile.TILE_SIZE, 0.25f, Tile.TILE_SIZE));
            }
        }

    }
}

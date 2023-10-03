using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    [ExecuteInEditMode]
    public class RoomModule : MonoBehaviour
    {
        public HashSet<Tile> Tiles;
        public HashSet<Tile> Excluded;
        public BoundsInt Bounds;

        private RoomFloor[] floors;

        private void Update()
        {
            if(PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                GenerateTiles();
                transform.position = Vector3.zero;
                transform.localScale = Vector3.one * Tile.TILE_SIZE;
            }
        }


        public void GenerateTiles()
        {
            Tiles = new HashSet<Tile>();
            Excluded = new HashSet<Tile>();
            floors = GetComponentsInChildren<RoomFloor>();

            foreach(RoomFloor floor in floors)
            {
                floor.GenerateTiles();

                if(floor.Exclude)
                {
                    Excluded.UnionWith(floor.Tiles);
                }
                else
                {                    
                    Tiles.UnionWith(floor.Tiles);
                }
            }

            Bounds = GetBounds();
        }

        private BoundsInt GetBounds(int offset = 0)
        {
            Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
            Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);

            foreach(Tile tile in Tiles)
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

            return new BoundsInt(min.x - offset, 0, min.y - offset, max.x - min.x + 1 + (offset * 2), 0, max.y - min.y + 1 + (offset * 2));
        }

                

        private void OnDrawGizmos()
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                return;
            }

            if (Tiles == null)
                return;

            float tileHalfSize = Tile.TILE_SIZE * 0.5f;

            foreach (var tile in Tiles)
            {
                Vector3 worldPosition = ((Vector3)tile);
                Gizmos.color = tile.IsCorridor ? Color.red : Color.green;
                Gizmos.DrawCube(transform.position + (worldPosition * Tile.TILE_SIZE) + new Vector3(tileHalfSize, 0.1f, tileHalfSize), new Vector3(Tile.TILE_SIZE - 0.1f, Tile.TILE_SIZE * 0.25f, Tile.TILE_SIZE - 0.1f));
            }

            var bounds = GetBounds();
            Gizmos.DrawWireCube((transform.position + bounds.center) * Tile.TILE_SIZE, new Vector3(bounds.size.x, bounds.size.y, bounds.size.z) * Tile.TILE_SIZE);

            // Center of room
            Gizmos.color = Color.grey;
            Gizmos.DrawCube(new Vector3(tileHalfSize, 0.1f, tileHalfSize), new Vector3(Tile.TILE_SIZE, 0.10f, Tile.TILE_SIZE));

            if (Excluded == null)
                return;

            foreach (var tile in Excluded)
            {
                Vector3 worldPosition = ((Vector3)tile) ;
                Gizmos.color = Color.black;
                Gizmos.DrawCube(transform.position + (worldPosition * Tile.TILE_SIZE) + new Vector3(tileHalfSize, 0.1f, tileHalfSize), new Vector3(Tile.TILE_SIZE - 0.1f, 0.25f, Tile.TILE_SIZE - 0.1f));
            }
        }
    }
}

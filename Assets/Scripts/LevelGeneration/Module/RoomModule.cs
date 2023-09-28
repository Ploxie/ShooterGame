using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.Test2
{
    [ExecuteInEditMode]
    public class RoomModule : MonoBehaviour
    {
        public HashSet<Tile> Tiles;

        private RoomFloor[] floors;
        public float tileSize = 1.0f;

        public BoundsInt Bounds;

        private void Update()
        {
            if(PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                GenerateTiles();
                transform.position = Vector3.zero;
                transform.localScale = Vector3.one;
            }
        }


        public void GenerateTiles()
        {
            Tiles = new HashSet<Tile>();
            floors = GetComponentsInChildren<RoomFloor>();
            foreach(RoomFloor floor in floors)
            {
                floor.GenerateTiles();
                Tiles.UnionWith(floor.Tiles);
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

                if (Tiles == null || Tiles.Count <= 0)
                return;

            

            foreach (var tile in Tiles)
            {
                Vector3 worldPosition = ((Vector3)tile) + new Vector3(0.5f, 0.1f, 0.5f);
                Gizmos.color = tile.IsCorridor ? Color.red : Color.green;
                Gizmos.DrawCube(transform.position + (worldPosition * tileSize), new Vector3(1.0f - 0.1f, 0.25f, 1.0f - 0.1f));
            }

            var bounds = GetBounds();
            Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);

            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(new Vector3(0.5f, 0.1f, 0.5f), new Vector3(1, 0.25f, 1));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.Test2
{
    [ExecuteInEditMode]
    public class RoomModule : MonoBehaviour
    {
        public HashSet<Tile> Tiles;
        public HashSet<Tile> Forbidden;

        private RoomFloor[] floors;
        public float tileSize = 1.0f;

        private void Update()
        {
            if(PrefabStageUtility.GetCurrentPrefabStage() != null)
                GenerateTiles();
        }

        public void GenerateTiles()
        {
            Tiles = new HashSet<Tile>();
            Forbidden = new HashSet<Tile>();
            floors = GetComponentsInChildren<RoomFloor>();
            foreach(RoomFloor floor in floors)
            {
                floor.GenerateTiles();
                if (!floor.exclude)
                {                   
                    Tiles.UnionWith(floor.Tiles);
                }
                else
                {
                    Forbidden.UnionWith(floor.Tiles);
                }
                
            }
        }

        private void OnDrawGizmos()
        {
            if (Tiles == null || Tiles.Count <= 0)
                return;

            foreach (var tile in Tiles)
            {
                Vector3 worldPosition = ((Vector3)tile) + new Vector3(0.5f, 0.1f, 0.5f);
                Gizmos.color = tile.IsCorridor ? Color.red : Color.green;
                Gizmos.DrawCube(transform.position + (worldPosition * tileSize), new Vector3(1.0f - 0.1f, 0.25f, 1.0f - 0.1f));
            }

            if (Forbidden == null || Forbidden.Count <= 0)
                return;

            foreach (var tile in Forbidden)
            {
                Vector3 worldPosition = ((Vector3)tile) + new Vector3(0.5f, 0.1f, 0.5f);
                Gizmos.color = Color.black;
                Gizmos.DrawCube(transform.position + (worldPosition * tileSize), new Vector3(1.0f - 0.1f, 0.25f, 1.0f - 0.1f));
            }
        }
    }
}

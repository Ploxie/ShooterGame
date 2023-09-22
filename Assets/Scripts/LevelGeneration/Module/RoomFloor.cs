using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.Test2
{
    [ExecuteInEditMode]
    public class RoomFloor : MonoBehaviour
    {

        [SerializeField] private Vector2Int size;
        [SerializeField] private Vector2Int offset;
        
        public HashSet<Tile> Tiles;
        private Transform mesh;

        public void GenerateTiles()
        {
            mesh = null;
            AddMesh();

            mesh.localScale = new Vector3(size.x, 0.25f, size.y);
            mesh.position = transform.position + new Vector3(size.x - (size.x * 0.5f) + offset.x, 0, size.y - (size.y * 0.5f) + offset.y);

            Tiles = new HashSet<Tile>();
            CalculateTiles();
        }

        private void CalculateTiles()
        {
            int width = size.x;
            int depth = size.y;
            for (int z = offset.y; z < depth + offset.y; z++)
            {
                for (int x = offset.x; x < width + offset.x; x++)
                {
                    Tiles.Add(new Tile(x,  z));
                }
            }
        }

        private void AddMesh()
        {
            if (mesh != null)
                return;

            var child = GetComponentInChildren<MeshRenderer>();
            if(child != null)
            {
                mesh = child.transform;
                return;
            }         


            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            {
                cube.transform.SetParent(transform, false);

            }

            mesh = cube.transform;
        }
    }
}

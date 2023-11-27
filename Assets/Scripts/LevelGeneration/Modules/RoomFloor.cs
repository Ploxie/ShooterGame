using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.LevelGeneration
{
    [ExecuteAlways]
    public class RoomFloor : MonoBehaviour
    {

        // ONLY IN PREFAB MODE
        private void Update() //REMOVE THIS WHEN BUILDING
        {
            //transform.position = new Vector3(
            //                Mathf.Round(transform.position.x / Tile.TILE_SIZE) * Tile.TILE_SIZE,
            //                 -0.625f,
            //                 Mathf.Round(transform.position.z / Tile.TILE_SIZE) * Tile.TILE_SIZE);

            //transform.localScale = new Vector3(
            //                Mathf.Round(transform.localScale.x / Tile.TILE_SIZE) * Tile.TILE_SIZE,
            //                 0.25f,
            //                 Mathf.Round(transform.localScale.z / Tile.TILE_SIZE) * Tile.TILE_SIZE);
        }

        public HashSet<Tile> CalculateTiles()
        {
            HashSet<Tile> tiles = new();

            Vector2 position = new Vector2(transform.position.x / Tile.TILE_SIZE, transform.position.z / Tile.TILE_SIZE);
            Vector2 size = new Vector2(transform.localScale.x / Tile.TILE_SIZE, transform.localScale.z / Tile.TILE_SIZE);

            for (float y = 0; y < size.y; y++)
            {
                for (float x = 0; x < size.x; x++)
                {
                    Vector2 tilePosition = new Vector2((position.x + x), (position.y + y));
                    tiles.Add(new Tile((int)tilePosition.x, (int)tilePosition.y));
                }
            }

            return tiles;
        }

    }

    public static class FloorCreateUtility
    {
        [MenuItem("GameObject/Level/Floor")]
        public static void CreateFloor(MenuCommand command)
        {
            GameObject floor = ObjectFactory.CreatePrimitive(PrimitiveType.Cube);
            floor.AddComponent<RoomFloor>();
            floor.name = "Floor";
            floor.isStatic = true;
            floor.transform.localScale = new Vector3(Tile.TILE_SIZE, 1.0f, Tile.TILE_SIZE);

            string assetPath = "Assets/Resources/Meshes/TileBase.asset";

            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
            
            if(mesh == null)
            {
                mesh = CreateTileBaseMesh();
                AssetDatabase.CreateAsset(mesh, assetPath);
            }

            floor.GetComponent<MeshFilter>().mesh = mesh;
            floor.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0.5f, 0.5f);           

        }
    
        private static Mesh CreateTileBaseMesh()
        {
            Mesh mesh = new Mesh();
            float length = 1f;
            float width = 1f;
            float height = 1f;


            //3) Define the co-ordinates of each Corner of the cube 
            Vector3[] c = new Vector3[8];

            c[0] = new Vector3(0, 0, height);
            c[1] = new Vector3(length, 0, height);
            c[2] = new Vector3(length, 0, 0);
            c[3] = new Vector3(0, 0, 0);

            c[4] = new Vector3(0, width, height);
            c[5] = new Vector3(length, width, height);
            c[6] = new Vector3(length, width, 0);
            c[7] = new Vector3(0, width, 0);


            //4) Define the vertices that the cube is composed of:
            //I have used 16 vertices (4 vertices per side). 
            //This is because I want the vertices of each side to have separate normals.
            //(so the object renders light/shade correctly) 
            Vector3[] vertices = new Vector3[]
            {
            c[0], c[1], c[2], c[3], // Bottom
	        c[7], c[4], c[0], c[3], // Left
	        c[4], c[5], c[1], c[0], // Front
	        c[6], c[7], c[3], c[2], // Back
	        c[5], c[6], c[2], c[1], // Right
	        c[7], c[6], c[5], c[4]  // Top
            };


            //5) Define each vertex's Normal
            Vector3 up = Vector3.up;
            Vector3 down = Vector3.down;
            Vector3 forward = Vector3.forward;
            Vector3 back = Vector3.back;
            Vector3 left = Vector3.left;
            Vector3 right = Vector3.right;


            Vector3[] normals = new Vector3[]
            {
            down, down, down, down,             // Bottom
	        left, left, left, left,             // Left
	        forward, forward, forward, forward,	// Front
	        back, back, back, back,             // Back
	        right, right, right, right,         // Right
	        up, up, up, up                      // Top
            };


            //6) Define each vertex's UV co-ordinates
            Vector2 uv00 = new Vector2(0f, 0f);
            Vector2 uv10 = new Vector2(1f, 0f);
            Vector2 uv01 = new Vector2(0f, 1f);
            Vector2 uv11 = new Vector2(1f, 1f);

            Vector2[] uvs = new Vector2[]
            {
                    uv11, uv01, uv00, uv10, // Bottom
	                uv11, uv01, uv00, uv10, // Left
	                uv11, uv01, uv00, uv10, // Front
	                uv11, uv01, uv00, uv10, // Back	        
	                uv11, uv01, uv00, uv10, // Right 
	                uv11, uv01, uv00, uv10  // Top
            };


            //7) Define the Polygons (triangles) that make up the our Mesh (cube)
            //IMPORTANT: Unity uses a 'Clockwise Winding Order' for determining front-facing polygons.
            //This means that a polygon's vertices must be defined in 
            //a clockwise order (relative to the camera) in order to be rendered/visible.
            int[] triangles = new int[]
            {
                    3, 1, 0,        3, 2, 1,        // Bottom	
	                7, 5, 4,        7, 6, 5,        // Left
	                11, 9, 8,       11, 10, 9,      // Front
	                15, 13, 12,     15, 14, 13,     // Back
	                19, 17, 16,     19, 18, 17,	    // Right
	                23, 21, 20,     23, 22, 21,     // Top
            };


            //8) Build the Mesh
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.Optimize();

            return mesh;
        }
    }
}

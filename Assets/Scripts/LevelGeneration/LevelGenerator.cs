using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {        
        [SerializeField] private int seed;
        [SerializeField] private ModuleManager moduleManager;


        private List<Room> rooms;
        private HashSet<Tile> tiles;
        private HashSet<Tile> excluded;
        private List<BoundsInt> boundsTest;

        private Room start;
        private Room end;

        private GameObject roomsObject;
        private GameObject floorsObject;
        private GameObject wallsObject;

        [SerializeField] private PhysicMaterial wallPhysicMaterial;

        [SerializeField]
        private NavMeshSurface navmesh;

        private void Awake()
        {
            moduleManager.Generate();
            Generate();
        }

        public void Generate()
        {
            // TODO: Add Walls in middle of rooms

            Random.InitState(seed);

            ClearTiles();
            tiles = new HashSet<Tile>();
            excluded = new HashSet<Tile>();
            boundsTest = new List<BoundsInt>();

            roomsObject = new GameObject("Rooms");
            {
                roomsObject.transform.parent = transform;
            }
            floorsObject = new GameObject("Floor");
            {
                floorsObject.transform.parent = transform;
                floorsObject.AddComponent<MeshFilter>();
            }
            wallsObject = new GameObject("Walls");
            {
                wallsObject.transform.parent = transform;
            }

            // Find Module Manager in child
            if(true){
                var child = GetComponentInChildren<ModuleManager>();
                if (child != null)
                    moduleManager = child;
            }

            // Generate Rooms
            if(true){
                rooms = GenerateRooms();
                foreach(Room room in rooms)
                {
                    tiles.UnionWith(SpawnRoomModule(room));
                }
            }

            // Generate Corridors
            if(true){
                HashSet<Tile> corridors = ConnectRooms(rooms);
                foreach(Tile tile in corridors)
                {
                    var room = new Room()
                    {
                        Position = tile.Position,
                        Module = moduleManager.CorridorModule
                    };
                    tiles.UnionWith(SpawnRoomModule(room));
                }

                tiles.ExceptWith(excluded);
            }

            // Generate Walls
            if(true){
                var walls = WallGenerator.Generate(tiles);
                tiles.UnionWith(walls);

                GenerateWalls(Tile.TILE_SIZE);
                
            }

            CombineFloorMeshes();

            BuildNavMesh();
        }

        public List<Room> GenerateRooms()
        {
            if(moduleManager == null || moduleManager.ModulePool?.Length <= 0)
                return new List<Room>();

            List<Room> rooms = new List<Room>();

            Vector2Int startPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

            // Add Start Module
            {
                var startModule = moduleManager.ModulePool[0];
                Room startRoom = new Room()
                {
                    Module = startModule,
                    Position = startPosition,
                };
                rooms.Add(startRoom);
                boundsTest.Add(startRoom.GetBounds(0));
                start = startRoom;
            }
           

            Room currentRoom = rooms[0];

            int attempts = 0;

            for(int i = 0; i < moduleManager.ModulePool.Length - 1; i++)
            {
                var module = moduleManager.ModulePool[i + 1];

                Room room = new Room()
                {
                    Module = module,
                };

                var direction = GetRandomValidDirection(currentRoom, room, rooms);
                var position = currentRoom.Position + GetOffset(0, direction);

                if (direction == Vector2Int.zero)
                {
                    i--;
                    currentRoom = currentRoom.Parent;
                    attempts++;
                    if(attempts > moduleManager.ModulePool.Length * moduleManager.ModulePool.Length || currentRoom == null)
                    {
                        Debug.Log("Something went wrong with the generation, please contact Level Generation Administrator");
                        break;
                    }
                    continue;
                }

                room.Parent = currentRoom;
                room.Position = position;

                rooms.Add(room);

                currentRoom = room;
                end = room;
            }

            return rooms;
        }

        private IEnumerable<Tile> SpawnRoomModule(Room room)
        {
            RoomModule module = Instantiate(room.Module, Vector3.zero, Quaternion.identity);
            module.GenerateTiles();
            Vector2Int roomCenter = room.Position;

            module.transform.position = new Vector3(roomCenter.x, 0, roomCenter.y) * Tile.TILE_SIZE;
            module.transform.SetParent(roomsObject.transform, true);

            HashSet<Tile> tiles = new HashSet<Tile>();
            foreach (var moduleTile in module.Tiles)
            {
                var position = roomCenter + moduleTile.Position;
                tiles.Add(new Tile(position) { IsModule = true });
            }
            foreach (var moduleTile in module.Excluded)
            {
                var position = roomCenter + moduleTile.Position;
                excluded.Add(new Tile(position) { IsModule = true });
            }

            return tiles;
        }

        private HashSet<Tile> ConnectRooms(List<Room> rooms)
        {
            HashSet<Tile> corridors = new HashSet<Tile>();
            int index = 0;
            for (int i = 1; i < rooms.Count; i++)
            {
                var currentRoom = rooms[index];
                var nextRoom = rooms[i];

                HashSet<Tile> corridor = CreateCorridor(currentRoom.GetRandomTile(), nextRoom.GetRandomTile());
                corridors.UnionWith(corridor);

                index = i;
            }
            return corridors;
        }

        private HashSet<Tile> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
        {
            HashSet<Tile> corridor = new HashSet<Tile>();
            var position = currentRoomCenter;
            corridor.Add(new Tile(position) { IsCorridor = true });

            while (position.y != destination.y)
            {
                if (destination.y > position.y)
                {
                    position += Vector2Int.up;
                }
                else if (destination.y < position.y)
                {
                    position += Vector2Int.down;
                }
                corridor.Add(new Tile(position) { IsCorridor = true });
            }
            while (position.x != destination.x)
            {
                if (destination.x > position.x)
                {
                    position += Vector2Int.right;
                }
                else if (destination.x < position.x)
                {
                    position += Vector2Int.left;
                }
                corridor.Add(new Tile(position) { IsCorridor = true });
            }
            
            
            return corridor;
        }

        public void GenerateWalls(float tileSize)
        {

            float wallHeight = tileSize;
            float wallThickness = tileSize / 10.0f;

            foreach (Tile tile in tiles)
            {
                float halfSize = tileSize * 0.5f;

                // Lol på denna koden, pallar inte snygga till den
                if (tile.HasWall(Tile.Wall.NORTH))
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    {
                        wall.transform.localScale = new Vector3(tileSize, wallHeight, wallThickness);
                        wall.transform.parent = wallsObject.transform;
                        wall.transform.position = new Vector3(tile.Position.x + 0.5f, 0.5f, tile.Position.y + 1) * Tile.TILE_SIZE;
                        wall.tag = "Wall";
                        wall.name = "Wall";
                        wall.isStatic = true;
                        wall.GetComponent<Collider>().material = wallPhysicMaterial;
                    }
                }
                if (tile.HasWall(Tile.Wall.SOUTH))
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    {
                        wall.transform.localScale = new Vector3(tileSize, wallHeight, wallThickness);
                        wall.transform.parent = wallsObject.transform;
                        wall.transform.position = new Vector3(tile.Position.x + 0.5f, 0.5f, tile.Position.y) * Tile.TILE_SIZE;
                        wall.transform.rotation = Quaternion.Euler(0, 180.0f, 0);
                        wall.tag = "Wall";
                        wall.name = "Wall";
                        wall.isStatic = true;
                        wall.GetComponent<Collider>().material = wallPhysicMaterial;
                    }
                }
                if (tile.HasWall(Tile.Wall.EAST))
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    {
                        wall.transform.localScale = new Vector3(tileSize, wallHeight, wallThickness);
                        wall.transform.parent = wallsObject.transform;
                        wall.transform.position = new Vector3(tile.Position.x + 1, 0.5f, tile.Position.y + 0.5f) * Tile.TILE_SIZE;
                        wall.transform.rotation = Quaternion.Euler(0, 90.0f, 0);
                        wall.tag = "Wall";
                        wall.name = "Wall";
                        wall.isStatic = true;
                        wall.GetComponent<Collider>().material = wallPhysicMaterial;
                    }
                }
                if (tile.HasWall(Tile.Wall.WEST))
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    {
                        wall.transform.localScale = new Vector3(tileSize, wallHeight, wallThickness);
                        wall.transform.parent = wallsObject.transform;
                        wall.transform.position = new Vector3(tile.Position.x, 0.5f, tile.Position.y + 0.5f) * Tile.TILE_SIZE;
                        wall.transform.rotation = Quaternion.Euler(0, -90.0f, 0);
                        wall.tag = "Wall";
                        wall.name = "Wall";
                        wall.isStatic = true;
                        wall.GetComponent<Collider>().material = wallPhysicMaterial;
                    }
                }
            }
        }

        private Vector2Int GetOffset(int offset, Vector2Int direction)
        {
            if (direction.x != 0)
                return new Vector2Int(direction.x + offset, direction.y);
            if (direction.y != 0)
                return new Vector2Int(direction.x, direction.y + offset);

            return direction;
        }

        private Vector2Int GetOverlap(int offset, Room from, Room to, Vector2Int direction)
        {
            var fromBounds = from.GetBounds(offset);
            var toBounds = to.GetBounds(offset);
            if (direction.x > 0)
            {
                int fromRight = fromBounds.size.x - (from.Position.x - fromBounds.xMin);
                int toLeft = to.Position.x - toBounds.xMin;
                return new Vector2Int(direction.x * (fromRight + toLeft), 0);
            }
            if (direction.x < 0)
            {
                int fromLeft = from.Position.x - fromBounds.xMin; 
                int toRight = toBounds.size.x - (to.Position.x - toBounds.xMin);
                return new Vector2Int(direction.x * (fromLeft + toRight), 0);
            }
            if (direction.y > 0)
            {
                int fromTop = fromBounds.size.z - (from.Position.y - fromBounds.zMin);
                int toBottom = to.Position.y - toBounds.zMin;
                return new Vector2Int(0, direction.y * (fromTop + toBottom));
            }
            if (direction.y < 0)
            {
                int fromBottom = from.Position.y - fromBounds.zMin;
                int toTop = toBounds.size.z - (to.Position.y - toBounds.zMin);
                return new Vector2Int(0, direction.y * (fromBottom + toTop));
            }

            return Vector2Int.zero;
        }

        private Vector2Int GetRandomValidDirection(Room from, Room to, IEnumerable<Room> invalidPositions)
        {
            List<Vector2Int> attempts = new List<Vector2Int>() { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };
            do
            {
                int index =  Random.Range(0, attempts.Count);

                Vector2Int direction = attempts[index];
                attempts.RemoveAt(index);

                Room test = new Room()
                {
                    Position = from.Position,
                    Module = to.Module
                };

                var offset = direction;
                var overlap = GetOverlap(1, from, test, direction);

                test.Position += overlap;
                var testBounds = test.GetBounds(1);

                bool contains = false;
                foreach(BoundsInt room in boundsTest)
                {
                    if(room.Intersects(testBounds))
                    {
                        contains = true;
                        break;
                    }
                }
                
                if (contains)
                    continue;
                boundsTest.Add(testBounds);
                return overlap;
            }
            while (attempts.Count > 0);

            return Vector2Int.zero;
        }

        private void ClearTiles()
        {
            var existing = GameObject.FindGameObjectsWithTag("Tile");
            var existingWalls = GameObject.FindGameObjectsWithTag("Wall");

            if (existing == null)
                return;
            if (existingWalls == null)
                return;


            //Array.ForEach(existing, child => {
            //    EditorApplication.delayCall += () => { DestroyImmediate(child); };
            //});
            //Array.ForEach(existingWalls, child => {
            //    EditorApplication.delayCall += () => { DestroyImmediate(child); };
            //});
        }

        private void CombineFloorMeshes()
        {
            RoomFloor[] floors = FindObjectsOfType<RoomFloor>();
            List<MeshFilter> meshFilters = new List<MeshFilter>();

            foreach(RoomFloor floor in floors)
            {
                MeshFilter filter = floor.GetComponent<MeshFilter>();
                if (filter != null)
                    meshFilters.Add(filter);
            }

            CombineInstance[] combine = new CombineInstance[meshFilters.Count];

            for (int i = 0; i < meshFilters.Count; i++)
            {
                
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
            }

            Mesh mesh = new Mesh();
            mesh.name = "Floors";
            mesh.CombineMeshes(combine);

            floorsObject.transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            floorsObject.transform.gameObject.SetActive(true);
        }

        private void BuildNavMesh()
        {
           
            navmesh.BuildNavMesh();
        }

        private void OnDrawGizmos()
        {
            if (rooms == null || rooms.Count <= 0)
                return;
           

            Vector3 north = new Vector3(0, 0, 1);
            Vector3 south = new Vector3(0, 0, -1);
            Vector3 east = new Vector3(1, 0, 0);
            Vector3 west = new Vector3(-1, 0, 0);

            float tileSize = Tile.TILE_SIZE;

            foreach (var tile in tiles)
            {
                Vector3 worldPosition = tile + new Vector3(0.5f, 0.25f, 0.5f);
                Gizmos.color = tile.IsCorridor ? Color.red : Color.cyan;
                Gizmos.DrawCube(transform.position + (worldPosition * tileSize), new Vector3(tileSize - 0.1f, 0.25f, tileSize - 0.1f));

                /*
                Gizmos.color = Color.blue;

                if (tile.HasWall(Tile.Wall.NORTH))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (north * 0.4f)) + (west * 0.4f) * tileSize), transform.position + ((worldPosition + (north * 0.4f)) + (east * 0.4f) * tileSize));

                if (tile.HasWall(Tile.Wall.SOUTH))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (south * 0.4f)) + (west * 0.4f) * tileSize), transform.position + ((worldPosition + (south * 0.4f)) + (east * 0.4f) * tileSize));

                if (tile.HasWall(Tile.Wall.EAST))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (east * 0.4f)) + (north * 0.4f) * tileSize), transform.position + ((worldPosition + (east * 0.4f)) + (south * 0.4f) * tileSize));

                if (tile.HasWall(Tile.Wall.WEST))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (west * 0.4f)) + (north * 0.4f) * tileSize), transform.position + ((worldPosition + (west * 0.4f)) + (south * 0.4f) * tileSize));
                */
            }

            foreach (BoundsInt bounds in boundsTest)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(bounds.center * Tile.TILE_SIZE, new Vector3(bounds.size.x, 1, bounds.size.z) * Tile.TILE_SIZE);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawCube(start.GetBounds(0).center, new Vector3(1, 2, 1));
            Gizmos.color = Color.red;
            Gizmos.DrawCube(end.GetBounds(0).center, new Vector3(1, 2, 1));
        }

    }

    public static class BoundsIntExt
    {
        public static bool Intersects(this BoundsInt a, BoundsInt b)
        {
            return (a.xMin < b.xMax) && (a.xMax > b.xMin) &&
                (a.yMin <= b.yMax) && (a.yMax >= b.yMin) &&
                (a.zMin < b.zMax) && (a.zMax > b.zMin);
        }
    }
}

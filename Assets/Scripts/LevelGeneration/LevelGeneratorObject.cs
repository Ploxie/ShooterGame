using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration.Test2
{
    [ExecuteInEditMode]
    public class LevelGeneratorObject : MonoBehaviour
    {
        [SerializeField] private int seed;
        [SerializeField] private float tileSize;
        [SerializeField] private RoomGenerator roomGenerator;
        [SerializeField] private CorridorGenerator corridorGenerator;
        [SerializeField] private WallGenerator wallGenerator;

        private HashSet<Tile> tiles;
        private HashSet<Tile> forbidden;
        private HashSet<Tile> corridors;
        private List<BoundsInt> roomsList;

        [SerializeField] private RoomModule[] roomModules;

        private void OnValidate()
        {
            Random.InitState(seed);
            ClearTiles();

            tiles = new HashSet<Tile>();

            forbidden = new HashSet<Tile>();

            // Create Room Bounds
            {
                Vector2Int levelPosition = Vector2Int.zero;
                Vector2Int levelSize = new Vector2Int(100, 100);
                int roomMinWidth = 20;
                int roomMinHeight = 20;
                roomsList = BinarySpacePartitioning.Generate(levelPosition, levelSize, roomMinWidth, roomMinHeight);
            }           


            tiles = CreateRoomsFromModules(roomsList);
            


            List<Vector2Int> roomCenters = new List<Vector2Int>();
            foreach (var room in roomsList)
            {
                roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(new Vector3(room.center.x, room.center.z, room.center.y)));
            }

            corridors = ConnectRooms(roomCenters);
            tiles.UnionWith(corridors);


            // Generate Walls
            {
                WallGenerator.Result walls = wallGenerator.Generate(tiles);
                tiles.UnionWith(walls.Positions);
            }

            foreach(Tile tile in corridors)
            {
                RoomModule module = Instantiate(roomModules[0], new Vector3(tile.Position.x - 0.5f, 0, tile.Position.y - 0.5f), Quaternion.identity);
                module.transform.SetParent(transform, true);
                module.GenerateTiles();

                foreach (var moduleTile in module.Tiles)
                {
                    var position = tile + moduleTile.Position;
                    tiles.Add(new Tile(position) { IsModule = false });
                }
            }

            tiles.ExceptWith(forbidden);

            // Remove DeadEnds
            {
                //tiles.ExceptWith(RemoveDeadEndTiles(tiles));

                // Need to regenerate walls
                //WallGenerator.Result walls = wallGenerator.Generate(tiles);
               // tiles.UnionWith(walls.Positions);
            }

            // Generate Doors
            {                
                //GenerateDoors(tiles);
            }

            //tiles.UnionWith(CreateRooms(bounds));

            /*CorridorGenerator.Result corridors = corridorGenerator.Generate(Vector2Int.zero);
            tiles.UnionWith(corridors.Positions);

            foreach(Vector2Int roomPosition in corridors.RoomPositions)
            {
                RoomGenerator.Result room = roomGenerator.Generate(roomPosition);
                tiles.UnionWith(room.Positions);
            }*/
        }

        private HashSet<Tile> CreateRooms(List<BoundsInt> rooms)
        {
            HashSet<Tile> floor = new HashSet<Tile>();
            int offset = 1;
            for (int i = 0; i < rooms.Count; i++)
            {
                var roomBounds = rooms[i];
                var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
                var roomFloor = CreateSimpleRooms(rooms, offset);// roomGenerator.Generate(roomCenter).Positions;
                foreach (var position in roomFloor)
                {
                    if (position.Position.x >= (roomBounds.xMin + offset) && position.Position.x <= (roomBounds.xMax - offset) && position.Position.y >= (roomBounds.yMin - offset) && position.Position.y <= (roomBounds.yMax - offset))
                    {
                        floor.Add(position);
                    }
                }
            }
            return floor;
        }

        private HashSet<Tile> CreateSimpleRooms(List<BoundsInt> roomsList, int offset)
        {
            HashSet<Tile> floor = new HashSet<Tile>();
            foreach (var room in roomsList)
            {
                for (int col = offset; col < room.size.x - offset; col++)
                {
                    for (int row = offset; row < room.size.y - offset; row++)
                    {
                        Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                        floor.Add(new Tile(position));
                    }
                }
            }
            return floor;
        }

        private HashSet<Tile> CreateRoomsRandomly(List<BoundsInt> roomsList)
        {
            HashSet<Tile> floor = new HashSet<Tile>();
            int offset = 1;
            for (int i = 0; i < roomsList.Count; i++)
            {
                var roomBounds = roomsList[i];
                var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
                var roomFloor = roomGenerator.Generate(roomCenter).Positions;
                foreach (var position in roomFloor)
                {
                    if (position.Position.x >= (roomBounds.xMin + offset) && position.Position.x <= (roomBounds.xMax - offset) && position.Position.y >= (roomBounds.yMin - offset) && position.Position.y <= (roomBounds.yMax - offset))
                    {
                        floor.Add(position);
                    }
                }
            }
            return floor;
        }

        private HashSet<Tile> CreateRoomsFromModules(List<BoundsInt> roomsList)
        {
            HashSet<Tile> floor = new HashSet<Tile>();

            if (roomModules.Length <= 0)
                return floor;

            for (int i = 0; i < roomsList.Count; i++)
            {
                var roomCenter = new Vector2Int(Mathf.RoundToInt(roomsList[i].center.x), Mathf.RoundToInt(roomsList[i].center.z));

                RoomModule module = Instantiate(roomModules[Random.Range(1,3)], new Vector3(roomCenter.x - 0.5f, 0, roomCenter.y - 0.5f), Quaternion.identity);
                module.transform.SetParent(transform, true);
                module.GenerateTiles();

                foreach (var moduleTile in module.Tiles)
                {
                    var position = roomCenter + moduleTile.Position;
                    floor.Add(new Tile(position) { IsModule = true});
                }
                foreach (var moduleTile in module.Forbidden)
                {
                    var position = roomCenter + moduleTile.Position;
                    forbidden.Add(new Tile(position) { IsModule = true });
                }
            }
            return floor;
        }

        private HashSet<Tile> ConnectRooms(List<Vector2Int> roomCenters)
        {
            HashSet<Tile> corridors = new HashSet<Tile>();
            var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
            roomCenters.Remove(currentRoomCenter);

            while (roomCenters.Count > 0)
            {
                Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
                roomCenters.Remove(closest);
                HashSet<Tile> newCorridor = CreateCorridor(currentRoomCenter, closest);
                currentRoomCenter = closest;
                corridors.UnionWith(newCorridor);
            }
            corridors.ExceptWith(forbidden);
            return corridors;
        }

        private HashSet<Tile> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
        {
            HashSet<Tile> corridor = new HashSet<Tile>();
            var position = currentRoomCenter;
            corridor.Add(new Tile(position) { IsCorridor = true});
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

        private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
        {
            Vector2Int closest = Vector2Int.zero;
            float distance = float.MaxValue;
            foreach (var position in roomCenters)
            {
                float currentDistance = Vector2.Distance(position, currentRoomCenter);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    closest = position;
                }
            }
            return closest;
        }

        private IEnumerable<Tile> RemoveDeadEndTiles(IEnumerable<Tile> tiles)
        {
            HashSet<Tile> deadEnds = new HashSet<Tile>(); 
            foreach(Tile tile in tiles)
            {
                if (!tile.IsModule && !tile.IsCorridor && tile.WallCount >= 3)
                    deadEnds.Add(tile);

            }
            return deadEnds;
        }


        private void OnDrawGizmos()
        {
            Vector3 north = new Vector3(0, 0, 1);
            Vector3 south = new Vector3(0, 0, -1);
            Vector3 east = new Vector3(1, 0, 0);
            Vector3 west = new Vector3(-1, 0, 0);

            float tileSize = 1.0f;

            foreach (var tile in tiles)
            {
                Vector3 worldPosition = tile - new Vector3(0.0f, -0.25f, 0.0f);
                Gizmos.color = tile.IsCorridor ? Color.red : Color.green;
                Gizmos.DrawCube(transform.position + (worldPosition * tileSize), new Vector3(1.0f - 0.1f, 0.25f, 1.0f - 0.1f));

                Gizmos.color = Color.blue;

                if (tile.HasWall(Tile.Wall.NORTH))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (north * 0.4f)) + (west * 0.4f) * tileSize), transform.position + ((worldPosition + (north * 0.4f)) + (east * 0.4f) * tileSize));

                if (tile.HasWall(Tile.Wall.SOUTH))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (south * 0.4f)) + (west * 0.4f) * tileSize), transform.position + ((worldPosition + (south * 0.4f)) + (east * 0.4f) * tileSize));

                if (tile.HasWall(Tile.Wall.EAST))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (east * 0.4f)) + (north * 0.4f) * tileSize), transform.position + ((worldPosition + (east * 0.4f)) + (south * 0.4f) * tileSize));

                if (tile.HasWall(Tile.Wall.WEST))
                    Gizmos.DrawLine(transform.position + ((worldPosition + (west * 0.4f)) + (north * 0.4f) * tileSize), transform.position + ((worldPosition + (west * 0.4f)) + (south * 0.4f) * tileSize));

            }

            foreach (var room in roomsList)
            {
                Random.InitState(room.GetHashCode());
                Gizmos.color = Random.ColorHSV();             
                Gizmos.DrawWireCube(new Vector3(room.center.x-0.5f, room.center.y, room.center.z-0.5f), new Vector3(room.size.x - 0.1f, room.size.y, room.size.z - 0.1f));
            }
        }

        private void ClearTiles()
        {
            var existing = GameObject.FindGameObjectsWithTag("Tile");

            if (existing == null)
                return;


            Array.ForEach(existing, child => {
                EditorApplication.delayCall += () => { DestroyImmediate(child); };
            });
        }

        public static HashSet<Tile> SimpleRandomWalk(Vector2Int start, int stepCount)
        {
            HashSet<Tile> result = new HashSet<Tile>();

            result.Add(new Tile(start));
            Vector2Int previous = start;

            for(int i = 0; i < stepCount - 1; i++)
            {
                Vector2Int current = previous + Direction2D.GetRandomCardinalDirection();
                Tile tile = new Tile(current)
                {
                    IsCorridor = false
                };
                result.Add(tile);
                previous = current;
            }

            return result;
        }


        public static List<Tile> RandomWalkCorridor(Vector2Int start, int stepCount)
        {
            List<Tile> corridor = new List<Tile>();
            var direction = Direction2D.GetRandomCardinalDirection();
            var current = start;
            for(int i = 0; i < stepCount; i++)
            {
                Tile tile = new Tile(current)
                {
                    IsCorridor = true
                };
                corridor.Add(tile);
                current += direction;
            }
            return corridor;
        }

    }
}

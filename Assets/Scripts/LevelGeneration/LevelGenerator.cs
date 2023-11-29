using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using static Assets.Scripts.LevelGeneration.Tile;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private Level level;

        [SerializeField] private int seed;
        [SerializeField, Range(1, 10)] private int spaceBetweenRooms = 1;
        [SerializeField, Range(0, 10)] private int randomWalkDrunkeness = 4;
        [SerializeField, Range(0.0f, 1.0f)] private float corridorRandomness = 0.5f;
        [SerializeField] private NavMeshSurface navMesh;
        private Material wallMaterial;

        private int corridorId = 0;

        [SerializeField] private bool isTutorial;

        public HashSet<Tile> tiles;
        public HashSet<Tile> corridors;
        public HashSet<Tile> doors;

        private Dictionary<RoomModule, WaveSpawner> waveSpawners;

        private void Awake()
        {
            tiles = new HashSet<Tile>();
            corridors = new HashSet<Tile>();
            doors = new HashSet<Tile>();
            waveSpawners = new();
            RoomManager.LoadPrefabs();
            seed = (int)Utils.GetUnixMillis();
            wallMaterial = Resources.Load<Material>("Materials/SeeThrough/SeeThrough");

        }

        private void Start()
        {
            if (isTutorial)
            {
                StartCoroutine((new[] {
                    GenerateTutorial(),
                    GenerateWalls(),
                    BuildNavMesh(),
                }).GetEnumerator());
                return;
            }


            UnityEngine.Random.InitState(seed);
            StartCoroutine((new[] {
                GenerateRoomLocations(),
                GenerateRoomModules(),
                GenerateCorridors(),
                GenerateWalls(),
                GenerateWaveRooms(),
                GenerateDoors(),
                PlaceKeys(),
                BuildNavMesh(),
                PlaceSpecialObjects()
            }).GetEnumerator());
        }

        private IEnumerator GenerateTutorial()
        {

            RoomModule[] rooms = GetComponentsInChildren<RoomModule>();
            foreach (var room in rooms)
            {

                var bounds = room.CalculateBounds();

                var roomCenter = new Vector2Int(bounds.x, bounds.y);
                foreach (var tile in room.Tiles)
                {
                    var t = new Tile(tile.Position) { IsModule = true, IsDoor = room.IsDoor, Previous = new Tile(tile.Position.x - 1, tile.Position.y) };
                    tiles.Add(t);
                    if (room.IsDoor)
                        doors.Add(t);
                }

            }

            yield return null;
        }


        public IEnumerator GenerateRoomLocations()
        {
            if (level == null)
                yield return null;

            // Clear Generation Data
            Reset(level.rootNode);

            Stack<Node> queue = new();
            queue.Push(level.rootNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Pop();

                if (currentNode is RoomNode)
                {
                    if (!GenerateRoomLocation(currentNode as RoomNode))
                    {
                        Reset(currentNode);
                        queue.Push(currentNode.Parent);
                    }
                }
                currentNode.Children.ForEach((c) => queue.Push(c));
                yield return null;
            }

        }

        private bool GenerateRoomLocation(RoomNode node)
        {
            var parent = node.GetParentRoom();
            if (parent == null)
            {
                node.Position = Vector2Int.zero;
                node.GeneratedModule = node.Module != null ? node.Module : RandomStartModule();
                node.Size = new(node.GeneratedModule.Size.x + spaceBetweenRooms, node.GeneratedModule.Size.y + spaceBetweenRooms);
                return true;
            }

            // This node has tried every direction and failed
            if (node.TriedEveryDirection())
            {
                node.triedDirections.Clear();
                return false;
            }

            Vector2Int position = parent.Position;
            Vector2Int direction = Direction2D.GetRandomCardinalDirection();

            RoomModule module = node.Module;
            if (module == null)
            {
                if (node == level.GetEndRoom())
                {
                    module = RandomEndModule();
                }
                else
                {
                    module = RandomRoomModule();
                }
            }

            Vector2Int moduleSize = new(module.Size.x + spaceBetweenRooms, module.Size.y + spaceBetweenRooms);
            Vector2Int parentModuleSize = parent.Size;
            Vector2Int spacing = direction * new Vector2Int(spaceBetweenRooms, spaceBetweenRooms);
            Vector2Int offset = Direction2D.GetPerpendicular(direction)[Random.Range(0, 1)] * randomWalkDrunkeness;

            node.triedDirections.Add(direction);

            direction *= (direction == Direction2D.NORTH || direction == Direction2D.EAST) ? parentModuleSize : moduleSize;
            direction += spacing;
            direction += offset;

            node.Position = position + direction;
            node.Size = moduleSize;

            if (!IsValidLocation(node))
            {
                return GenerateRoomLocation(node);
            }


            node.GeneratedModule = module;

            return true;
        }

        private bool IsValidLocation(RoomNode node)
        {
            Queue<Node> queue = new();
            queue.Enqueue(level.rootNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                RoomNode room = currentNode as RoomNode;
                if (room)
                {
                    if (room.Size != Vector2Int.zero && room != node)
                    {
                        if (node.Intersects(room))
                            return false;
                    }
                }

                currentNode.Children.ForEach((c) => queue.Enqueue(c));
            }

            return true;
        }

        private void Reset(Node node)
        {
            Queue<Node> queue = new();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                currentNode.Size = Vector2Int.zero;
                currentNode.Position = Vector2Int.zero;

                RoomNode roomNode = currentNode as RoomNode;
                if (roomNode != null)
                    roomNode.GeneratedModule = roomNode.Module;

                currentNode.Children.ForEach((c) => queue.Enqueue(c));
            }
        }

        private IEnumerator GenerateRoomModules()
        {
            Queue<Node> queue = new();
            queue.Enqueue(level.rootNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();


                RoomNode roomNode = currentNode as RoomNode;
                if (roomNode != null)
                {
                    RoomModule module = Instantiate(roomNode.GeneratedModule, Vector3.zero, Quaternion.identity);
                    module.CalculateBounds();
                    Vector2Int roomCenter = roomNode.Position;

                    module.transform.position = new Vector3(roomCenter.x - module.Bounds.x, 0, roomCenter.y - module.Bounds.z) * Tile.TILE_SIZE;
                    module.transform.SetParent(transform, true);

                    foreach (var tile in module.Tiles)
                    {
                        tiles.Add(new Tile(roomCenter - new Vector2Int(module.Bounds.x, module.Bounds.z) + tile.Position) { IsModule = true });
                    }

                    roomNode.CreatedModule = module;
                }


                currentNode.Children.ForEach((c) => queue.Enqueue(c));

                yield return null;
            }

        }

        private IEnumerator GenerateCorridors()
        {
            Queue<Node> queue = new();
            queue.Enqueue(level.rootNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                RoomNode roomNode = currentNode as RoomNode;
                if (roomNode != null)
                {
                    var parent = roomNode.GetParentRoom();

                    if (parent != null)
                    {
                        var corridor = GenerateCorridor(roomNode, parent);

                        tiles.UnionWith(corridor);
                    }
                }

                currentNode.Children.ForEach((c) => queue.Enqueue(c));
                yield return null;
            }

            foreach (Tile tile in tiles)
            {
                if (tile.IsCorridor)
                {
                    RoomModule module = Instantiate(RandomCorridorModule(), Vector3.zero, Quaternion.identity);
                    module.CalculateBounds();

                    module.transform.position = new Vector3(tile.Position.x, 0, tile.Position.y) * Tile.TILE_SIZE;
                    module.transform.SetParent(transform, true);
                }
            }

        }

        private HashSet<Tile> GenerateCorridor(RoomNode node, RoomNode parent)
        {
            HashSet<Tile> corridor = new HashSet<Tile>();

            var connectionTiles = FindClosestTiles(node, parent);

            var position = connectionTiles.Item2 - new Vector2Int(parent.GeneratedModule.Bounds.x, parent.GeneratedModule.Bounds.z) + parent.Position;
            var destination = connectionTiles.Item1 - new Vector2Int(node.GeneratedModule.Bounds.x, node.GeneratedModule.Bounds.z) + node.Position;

            Tile previous = null;

            int id = corridorId++;

            bool doorPlaced = false;
            DoorNode door = node.GetDoorNode();

            {
                Tile tile = new Tile(position) { IsCorridor = true, id = id };
                corridor.Add(tile);
                previous = tile;
            }

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
                Tile tile = new Tile(position) { IsCorridor = true, Previous = previous, id = id };
                if (door != null && !doorPlaced && !tiles.Contains(tile))
                {
                    tile.IsDoor = door != null;
                    tile.keyType = door != null ? door.Key : Key.KeyType.None;
                    doors.Add(tile);
                    doorPlaced = true;
                }
                if ((node.IsWaveRoom || parent.IsWaveRoom) && !doorPlaced && !tiles.Contains(tile))
                {
                    tile.IsDoor = true;
                    tile.keyType = node.IsWaveRoom ? Key.KeyType.None : Key.KeyType.Boss;
                    tile.isWave = true;
                    tile.room = node.IsWaveRoom ? node.GeneratedModule : parent.GeneratedModule;
                    doors.Add(tile);
                    doorPlaced = true;
                }
                corridor.Add(tile);
                previous = tile;
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
                Tile tile = new Tile(position) { IsCorridor = true, Previous = previous, id = id };
                if (door != null && !doorPlaced && !tiles.Contains(tile))
                {
                    tile.IsDoor = door != null;
                    tile.keyType = door != null ? door.Key : Key.KeyType.None;
                    doors.Add(tile);
                    doorPlaced = true;
                }
                if ((node.IsWaveRoom || parent.IsWaveRoom) && !doorPlaced && !tiles.Contains(tile)) // Entry
                {
                    tile.IsDoor = true;
                    tile.keyType = node.IsWaveRoom ? Key.KeyType.None : Key.KeyType.Boss;

                    tile.isWave = true;
                    tile.room = node.IsWaveRoom ? node.GeneratedModule : parent.GeneratedModule;
                    doors.Add(tile);
                    doorPlaced = true;
                }
                corridor.Add(tile);
                previous = tile;
            }
            return corridor;
        }

        private (Vector2Int, Vector2Int) FindClosestTiles(RoomNode first, RoomNode second)
        {
            Vector2Int closestF = first.GeneratedModule.Tiles.ElementAt((int)Random.value * first.GeneratedModule.Tiles.Count) - new Vector2Int(first.GeneratedModule.Bounds.x, first.GeneratedModule.Bounds.z) + first.Position;
            Vector2Int closestS = second.GeneratedModule.Tiles.ElementAt((int)Random.value * second.GeneratedModule.Tiles.Count) - new Vector2Int(second.GeneratedModule.Bounds.x, second.GeneratedModule.Bounds.z) + second.Position; ;

            int closest = Math.Abs(closestS.x - closestF.x) + Math.Abs(closestS.y - closestF.y) + (int)(Random.value * corridorRandomness * first.GeneratedModule.Tiles.Count);

            foreach (var f in first.GeneratedModule.Tiles)
            {
                foreach (var s in second.GeneratedModule.Tiles)
                {
                    var fPos = f - new Vector2Int(first.GeneratedModule.Bounds.x, first.GeneratedModule.Bounds.z) + first.Position;
                    var sPos = s - new Vector2Int(second.GeneratedModule.Bounds.x, second.GeneratedModule.Bounds.z) + second.Position;

                    var randomness = (int)(Random.value * corridorRandomness * first.GeneratedModule.Tiles.Count);
                    var distance = Math.Abs(sPos.x - fPos.x) + Math.Abs(sPos.y - fPos.y) + randomness;
                    if (closest > distance)
                    {
                        closest = distance;
                        closestF = f;
                        closestS = s;
                    }
                }
            }

            return (closestF, closestS);
        }

        private IEnumerator GenerateWalls()
        {


            foreach (Tile tile in tiles)
            {
                foreach (var direction in Direction2D.CARDINAL)
                {
                    var neighbourPosition = new Tile(tile.Position + direction);

                    if (!tiles.TryGetValue(neighbourPosition, out Tile neighbour) || (tile.id != neighbour.id && tile.IsCorridor && neighbour.IsCorridor))
                    {
                        tile.Walls |= Wall.FromDirection(direction);
                    }
                }

                if (tile.IsDoor)
                {
                    //var direction = tile.Previous.Position - tile.Position;
                    //tile.Walls |= Wall.FromDirection(direction) << 0x4;                    
                }

                if (tile.HasWall(Wall.NORTH))
                    CreateWall(tile, Wall.NORTH);
                if (tile.HasWall(Wall.SOUTH))
                    CreateWall(tile, Wall.SOUTH);
                if (tile.HasWall(Wall.EAST))
                    CreateWall(tile, Wall.EAST);
                if (tile.HasWall(Wall.WEST))
                    CreateWall(tile, Wall.WEST);
            }

            yield return null;
        }

        private IEnumerator GenerateDoors()
        {

            foreach (Tile tile in doors)
            {
                var direction = tile.Previous.Position - tile.Position;
                tile.Walls |= Wall.FromDirection(direction) << 0x4;

                Debug.Log("Creating door at: " + tile.Position);
                CreateDoor(tile, Wall.NORTH_DOOR);
                CreateDoor(tile, Wall.SOUTH_DOOR);
                CreateDoor(tile, Wall.EAST_DOOR);
                CreateDoor(tile, Wall.WEST_DOOR);
            }

            yield return null;
        }

        private IEnumerator PlaceKeys()
        {
            foreach (var node in level.nodes)
            {
                RoomNode roomNode = node as RoomNode;
                if (roomNode != null)
                {
                    if (roomNode.HasKey)
                    {
                        var prefabPath = "Prefabs/Keys/" + roomNode.Key + "Key";
                        Key prefab = Resources.Load<Key>(prefabPath);

                        Key key = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                        {
                            key.transform.parent = transform;
                            
                            var keySpawns = roomNode.CreatedModule.GetComponentsInChildren<EnemySpawner>(true);
                            var position = new Vector3(roomNode.Position.x + (roomNode.Size.x * 0.5f), 0, roomNode.Position.y + (roomNode.Size.y * 0.5f)) * Tile.TILE_SIZE;

                            key.transform.position = keySpawns != null ? keySpawns[(int)(Random.value * keySpawns.Length)].transform.position : position;
                            key.SetKeyType(roomNode.Key);
                            Debug.Log("Placed " + roomNode.Key + " Key at " + key.transform.position);
                        }
                    }
                }
            }

            yield return null;
        }

        private IEnumerator GenerateWaveRooms()
        {
            foreach (var node in level.nodes)
            {
                RoomNode roomNode = node as RoomNode;
                if (roomNode != null && roomNode.IsWaveRoom)
                {
                    RoomModule module = roomNode.CreatedModule;
                    var spawnPoints = module.GetComponentsInChildren<EnemySpawner>(true);

                    var waveSpawner = new GameObject("Wave Spawner").AddComponent<WaveSpawner>();
                    //waveSpawner.transform.parent = module.transform;
                    waveSpawner.spawnPoints = new();
                    foreach (var spawnPoint in spawnPoints)
                    {
                        waveSpawner.spawnPoints.Add(spawnPoint.gameObject);
                        spawnPoint.gameObject.SetActive(false);
                    }


                    waveSpawner.Waves = new Wave[3];
                    var enemyPrefabs = new[] {
                        Resources.Load<Entity.Enemy>("Prefabs/Enemy/EnemyKamikaze - Axel 1"),
                        Resources.Load<Entity.Enemy>("Prefabs/Enemy/EnemyMelee - Axel 1"),
                        Resources.Load<Entity.Enemy>("Prefabs/Enemy/EnemyRangedDisolve")
                    };

                    int[] numberOfEnemiesPerWave = { 5, 10, 15};

                    for (int i = 0; i < waveSpawner.Waves.Length; i++)
                    {
                        waveSpawner.Waves[i] = new Wave();
                        waveSpawner.Waves[i].Enemies = new Entity.Enemy[numberOfEnemiesPerWave[i]];
                        for (int j = 0; j < waveSpawner.Waves[i].Enemies.Length; j++)
                        {
                            waveSpawner.Waves[i].Enemies[j] = enemyPrefabs[Random.Range(0, 3)];
                        }
                    }

                    waveSpawner.gameObject.SetActive(false);
                    waveSpawners.TryAdd(roomNode.GeneratedModule, waveSpawner);

                }
            }


            yield return null;
        }

        private IEnumerator BuildNavMesh()
        {
            navMesh.BuildNavMesh();
            yield return null;
        }
        private IEnumerator PlaceSpecialObjects()
        {
            GameObject[] specialEnemySpawnPoints = GameObject.FindGameObjectsWithTag("SpecialEnemySpawnPoint");
            EnemySpawner enemySpawner = Resources.Load<EnemySpawner>("Prefabs/Enemy/EnemySpawner");

            int numberOfSpecialEnemySpawns = 5;
            int specialEnemyRange = specialEnemySpawnPoints.Length / numberOfSpecialEnemySpawns;
            for (int i = 0; i < specialEnemyRange * numberOfSpecialEnemySpawns; i += specialEnemyRange)
            {
                int index = i + (int)((specialEnemyRange - 1) * Random.value);
                EnemySpawner enemySpawner1 = Instantiate(enemySpawner, specialEnemySpawnPoints[index].transform);
                enemySpawner1.SpawnSpecialEnemy = true;
                enemySpawner1.ContinousSpawns = false;
                enemySpawner1.Activate();
            }

            GameObject[] powerUpSpawnPoints = GameObject.FindGameObjectsWithTag("PowerUpSpawnPoint");
            PowerUpPickUp powerUpPickUp = Resources.Load<PowerUpPickUp>("Prefabs/Pickups/PowerUp");

            int numberOfPickUpSpawns = 5;
            int powerUprange = powerUpSpawnPoints.Length / numberOfPickUpSpawns;
            for (int i = 0; i < powerUprange * numberOfPickUpSpawns; i += powerUprange)
            {
                int index = i + (int)((powerUprange - 1) * Random.value);
                PowerUpPickUp powerUpPickUp1 = Instantiate(powerUpPickUp);
                powerUpPickUp1.transform.position = powerUpSpawnPoints[index].transform.position;
            }

            yield return null;
        }

        private GameObject CreateWall(Tile tile, int mask)
        {


            if (!tile.HasWall(mask))
            {
                return null;
            }

            float tileSize = TILE_SIZE + 0.5f;
            float wallHeight = tileSize * 0.75f;
            float wallThickness = tileSize / 10.0f;

            float offsetX = mask == Wall.EAST ? 1.0f : 0.0f;
            offsetX = mask == Wall.NORTH || mask == Wall.SOUTH ? 0.5f : offsetX;

            float offsetY = mask == Wall.NORTH ? 1.0f : 0.0f;
            offsetY = mask == Wall.EAST || mask == Wall.WEST ? 0.5f : offsetY;

            float rotation = mask == Wall.SOUTH ? 180.0f : 0.0f;
            rotation = mask == Wall.EAST ? 90.0f : rotation;
            rotation = mask == Wall.WEST ? -90.0f : rotation;

            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            {
                wall.transform.localScale = new Vector3(tileSize, wallHeight, wallThickness);
                wall.transform.position = new Vector3((tile.Position.x + offsetX) * Tile.TILE_SIZE, 0.75f, (tile.Position.y + offsetY) * Tile.TILE_SIZE);

                wall.transform.rotation = Quaternion.Euler(0, rotation, 0);
                wall.transform.SetParent(transform);
                wall.tag = "Wall";
                wall.name = "Wall";
                wall.isStatic = true;
                wall.layer = 9;
                //wall.GetComponent<Collider>().material = wallPhysicMaterial;
            }

            if (wall.TryGetComponent(out MeshRenderer renderer))
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.material = Resources.Load<Material>("Materials/Office/Ground Mats/Epoxy/Epoxy Ground 20m");
            }

            return wall;
        }

        private GameObject CreateDoor(Tile tile, int mask)
        {
            if (!tile.HasWall(mask))
            {
                return null;
            }

            var prefabPath = "Prefabs/Door/Door";

            if (tile.isWave && tile.keyType == Key.KeyType.None)
            {
                prefabPath = "Prefabs/Door/BossDoor";
            }

            GameObject prefab = Resources.Load<GameObject>(prefabPath);


            if (prefab == null)
            {
                Debug.LogError("Could not find Door prefab at location: " + prefabPath);
                return null;
            }



            float tileSize = TILE_SIZE;
            float wallHeight = tileSize;
            float wallThickness = tileSize / 10.0f;

            float offsetX = mask == Wall.EAST_DOOR ? 1.0f : 0.0f;
            offsetX = mask == Wall.NORTH_DOOR || mask == Wall.SOUTH_DOOR ? 0.5f : offsetX;

            float offsetY = mask == Wall.NORTH_DOOR ? 1.0f : 0.0f;
            offsetY = mask == Wall.EAST_DOOR || mask == Wall.WEST_DOOR ? 0.5f : offsetY;

            float rotation = mask == Wall.SOUTH_DOOR ? 180.0f : 0.0f;
            rotation = mask == Wall.EAST_DOOR ? 90.0f : rotation;
            rotation = mask == Wall.WEST_DOOR ? -90.0f : rotation;

            GameObject door = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            {
                door.transform.localScale = new Vector3(0.75f, 0.6f, 0.6f);
                door.transform.parent = transform;
                door.transform.position = new Vector3((tile.Position.x + offsetX) * Tile.TILE_SIZE, -0.5f, (tile.Position.y + offsetY) * Tile.TILE_SIZE);
                door.transform.rotation = Quaternion.Euler(0, rotation + 90.0f, 0);
                door.GetComponentInChildren<Door>().SetKeyType(tile.keyType);
            }
            if (tile.isWave)
            {
                if (waveSpawners.TryGetValue(tile.room, out WaveSpawner spawner))
                {
                    if (door.GetComponentInChildren<CloseDoor>() != null)
                    {
                        door.GetComponentInChildren<CloseDoor>().WaveSpawner = spawner.gameObject;
                    }
                        
                    if (spawner.doors == null)
                    {
                        spawner.doors = new();
                    }
                    spawner.doors.Add(door);
                }
            }
            return door;
        }

        public RoomModule RandomRoomModule()
        {
            return RoomManager.RoomModules[(int)(Random.value * RoomManager.RoomModules.Length)];
        }
        public RoomModule RandomStartModule()
        {
            return RoomManager.StartModules[(int)(Random.value * RoomManager.StartModules.Length)];
        }
        public RoomModule RandomEndModule()
        {
            return RoomManager.EndModules[(int)(Random.value * RoomManager.EndModules.Length)];
        }
        public RoomModule RandomCorridorModule()
        {
            return RoomManager.CorridorModules[(int)(Random.value * RoomManager.CorridorModules.Length)];
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            if (level != null)
            {
                foreach (Node node in level.nodes)
                {
                    var position = new Vector3(node.Position.x + (node.Size.x * 0.5f), 0, node.Position.y + (node.Size.y * 0.5f)) * Tile.TILE_SIZE;

                    Gizmos.DrawWireCube(position, new Vector3(node.Size.x, 0, node.Size.y) * Tile.TILE_SIZE);

                    RoomNode roomNode = node as RoomNode;
                    if (roomNode != null && roomNode.GeneratedModule != null)
                        Handles.Label(position, "" + roomNode.GeneratedModule.name);

                    if (node.Parent == null)
                        continue;

                    var parentPosition = new Vector3(node.Parent.Position.x + (node.Parent.Size.x * 0.5f), 0, node.Parent.Position.y + (node.Parent.Size.y * 0.5f)) * Tile.TILE_SIZE;

                    Gizmos.DrawLine(position, parentPosition);
                }
            }

            if (tiles == null)
                return;


            foreach (var tile in tiles)
            {
                if (tile == null)
                    return;

                Gizmos.color = new Color(0.1f, 1.0f, 0.1f, 0.1f);
                if (tile.IsCorridor)
                    Gizmos.color = Color.red;
                Vector3 worldPosition = tile + new Vector3(0.5f, 0.0f, 0.5f);
                Gizmos.DrawWireCube(worldPosition * Tile.TILE_SIZE, new Vector3(Tile.TILE_SIZE, 0f, Tile.TILE_SIZE));

            }

            foreach (Tile tile in doors)
            {
                Gizmos.color = Color.blue;

                Vector3 north = new Vector3(0, 0, 1);
                Vector3 south = new Vector3(0, 0, -1);
                Vector3 east = new Vector3(1, 0, 0);
                Vector3 west = new Vector3(-1, 0, 0);

                Vector3 worldPosition = tile + new Vector3(0.5f, 0.0f, 0.5f);

                Gizmos.DrawCube(worldPosition * Tile.TILE_SIZE, new Vector3(Tile.TILE_SIZE, 0f, Tile.TILE_SIZE));

                if (tile.HasWall(Tile.Wall.NORTH_DOOR))
                    Gizmos.DrawLine(((worldPosition + (north * 0.4f)) + (west * 0.4f)) * TILE_SIZE, ((worldPosition + (north * 0.4f)) + (east * 0.4f)) * TILE_SIZE);

                if (tile.HasWall(Tile.Wall.SOUTH_DOOR))
                    Gizmos.DrawLine(((worldPosition + (south * 0.4f)) + (west * 0.4f)) * TILE_SIZE, ((worldPosition + (south * 0.4f)) + (east * 0.4f)) * TILE_SIZE);

                if (tile.HasWall(Tile.Wall.EAST_DOOR))
                    Gizmos.DrawLine(((worldPosition + (east * 0.4f)) + (north * 0.4f)) * TILE_SIZE, ((worldPosition + (east * 0.4f)) + (south * 0.4f)) * TILE_SIZE);

                if (tile.HasWall(Tile.Wall.WEST_DOOR))
                    Gizmos.DrawLine(((worldPosition + (west * 0.4f)) + (north * 0.4f)) * TILE_SIZE, ((worldPosition + (west * 0.4f)) + (south * 0.4f)) * TILE_SIZE);
            }

        }
    }
}

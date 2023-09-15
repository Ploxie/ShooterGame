using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;


[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    
    [SerializeField] private int seed;

    [SerializeField] private int tileCount;

    [SerializeField] private float tileSize = 5.0f;

    [SerializeField, Range(0.0f, 1.0f)] private float adjacentConnectionChance;

    [SerializeField] private int branchCount;
    [SerializeField, Range(1, 10)] private int branchMinLength;
    [SerializeField, Range(1, 10)] private int branchMaxLength;

    [SerializeField] private bool renderFloor;

    private Dictionary<Vector2, Tile> tiles;
    private List<Wall> doors;
    private Tile startTile;
    private Tile endTile;

    void OnValidate()
    {
        Random.InitState(seed);

        ClearTiles();
        startTile = null;
        endTile = null;

        tiles = new Dictionary<Vector2, Tile>();
        doors = new List<Wall>();

        startTile = new Tile(transform.position / tileSize, tileSize);
        startTile.IsMainBranch = true;
        tiles.Add(transform.position / tileSize, startTile);

        GenerateBranch(startTile, tileCount - 1, true); // MAIN BRANCH

        GenerateBranches();
        GenerateConnections();
        GenerateDoors();
        //GenerateRooms();
        GenerateMeshes();
    }

    private void GenerateBranch(Tile startTile, int branchLength, bool mainBranch)
    {
        if(branchLength <= 0)
            return;

        Tile previousTile = startTile;

        for (int i = 0; i < branchLength; ++i)
        {
            if (previousTile == null && !mainBranch)
                break;

            Vector2 direction = GenerateRandomDirection(previousTile.Position);
            Vector2 position = previousTile.Position + direction;

            if(position == previousTile.Position || !mainBranch && (position == endTile.Position || position == startTile.Position))
            {
                i -= 1; // GO BACK TO PREVIOUS TILE
                previousTile = previousTile.Parent;
                continue;                
            }


            Tile currentTile = new Tile(position, tileSize);
            {
                currentTile.Parent = previousTile;
                currentTile.AddNeighbour(previousTile);
                currentTile.DistanceFromStart = previousTile.DistanceFromStart + 1;
                currentTile.IsMainBranch = mainBranch;
            }

            previousTile.AddNeighbour(currentTile);   
            tiles.Add(position, currentTile);

            previousTile = currentTile;

            if (mainBranch)
                endTile = currentTile;
        }        

    }

    private void GenerateBranches()
    {
        if (branchCount <= 0)
            return;

        if (tiles.Count <= 2) // Need more than Start- and End tiles
            return;

        for(int branchIndex = 0; branchIndex < branchCount; ++branchIndex)
        {
            Tile branchStart;
            do
            {
                branchStart = tiles.ElementAt(Random.Range(1, tiles.Count - 1)).Value;
            }
            while (branchStart == startTile || branchStart == endTile);

            int branchMinSize = Math.Min(branchMinLength, branchMaxLength);
            int branchMaxSize = Math.Min(branchMinLength, branchMaxLength);
            int branchSize = Random.Range(branchMinSize, branchMaxSize);

            GenerateBranch(branchStart, branchSize, false);
        }
    }

    private void GenerateConnections()
    {
        foreach(var entry in tiles)
        {
            Vector2 tilePosition = entry.Key;
            Tile tile = entry.Value;

            List<Tile> neighbours = FindNeighbours(tilePosition);

            foreach(Tile neighbour in neighbours)
            {
                if(neighbour == startTile || tile == startTile || neighbour == endTile || tile == endTile)
                    continue;

                if (tile.IsMainBranch != neighbour.IsMainBranch) // We don't want branches to connect to the main branch
                    continue;

                int distanceFromStartDifference = Math.Abs(tile.DistanceFromStart - neighbour.DistanceFromStart);

                if (distanceFromStartDifference > 6) // Limit massive shortcuts, magic number should maybe be a variable
                    continue;

                float connectionRoll = Random.Range(0.0f, 1.0f);
                if (connectionRoll <= adjacentConnectionChance)
                {
                    tile.AddNeighbour(neighbour);            
                }
            }
        }
    }

    private void GenerateDoors()
    {
        foreach(Tile tile in tiles.Values)
        {
            if(tile.IsCorridor)
            {              

                float doorRoll = Random.Range(0.0f, 1.0f);

                if(doorRoll <= 0.25f) // 50% chance to spawn a door at corridors
                {
                    tile.DoorMask = tile.NeighbourMask;

                    if (tile.HasDoors(Tile.NORTH_MASK))
                        doors.Add(new Wall(tile.Position, tile.Position + new Vector2(0, 1)));
                    if (tile.HasDoors(Tile.SOUTH_MASK))
                        doors.Add(new Wall(tile.Position, tile.Position + new Vector2(0, -1)));
                    if (tile.HasDoors(Tile.EAST_MASK))
                        doors.Add(new Wall(tile.Position, tile.Position + new Vector2(1, 0)));
                    if (tile.HasDoors(Tile.WEST_MASK))
                        doors.Add(new Wall(tile.Position, tile.Position + new Vector2(-1, 0)));
                }                
            }
        }
    }

    private void GenerateMeshes()
    {
        foreach(var entry in tiles)
        {
            Vector2 tilePosition = entry.Key;
            Tile tile = entry.Value;

            Vector3 tileWorldPosition = new Vector3(tilePosition.x, 0, tilePosition.y) * tileSize;

            if(renderFloor)
            {
                GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                {
                    floor.transform.localScale = new Vector3(tileSize, tileSize / 25.0f, tileSize);
                    floor.transform.parent = transform;
                    floor.transform.localPosition = tileWorldPosition;
                    floor.tag = "Tile";
                    floor.name = "Floor";
                    floor.gameObject.isStatic = true;
                }
            }               

            if (!tile.HasDoors(Tile.NORTH_MASK) && !tile.HasNeighbours(Tile.NORTH_MASK))
            {
                Wall wall = new Wall(Wall.Cardinal.NORTH);
                wall.GenerateMesh(tileSize, new Vector3(tile.Position.x, 0, tile.Position.y + 0.5f) * tileSize, transform);
            }
            if (!tile.HasDoors(Tile.SOUTH_MASK) && !tile.HasNeighbours(Tile.SOUTH_MASK))
            {
                Wall wall = new Wall(Wall.Cardinal.SOUTH);
                wall.GenerateMesh(tileSize, new Vector3(tile.Position.x, 0, tile.Position.y - 0.5f) * tileSize, transform);
            }
            if (!tile.HasDoors(Tile.EAST_MASK) && !tile.HasNeighbours(Tile.EAST_MASK))
            {
                Wall wall = new Wall(Wall.Cardinal.EAST);
                wall.GenerateMesh(tileSize, new Vector3(tile.Position.x + 0.5f, 0, tile.Position.y) * tileSize, transform);
            }
            if (!tile.HasDoors(Tile.WEST_MASK) && !tile.HasNeighbours(Tile.WEST_MASK))
            {
                Wall wall = new Wall(Wall.Cardinal.WEST);
                wall.GenerateMesh(tileSize, new Vector3(tile.Position.x - 0.5f, 0, tile.Position.y) * tileSize, transform);
            }
        }

        foreach (Wall door in doors)
        {
            Vector2 doorPosition = door.PositionA + ((door.PositionB - door.PositionA) * 0.5f);

            Tile a = tiles[door.PositionA];
            Tile b = tiles[door.PositionB];

            int mask = 0;
            mask |= door.Orientation == Wall.Cardinal.NORTH ? Tile.NORTH_MASK : 0;
            mask |= door.Orientation == Wall.Cardinal.SOUTH ? Tile.SOUTH_MASK : 0;
            mask |= door.Orientation == Wall.Cardinal.EAST ? Tile.EAST_MASK : 0;
            mask |= door.Orientation == Wall.Cardinal.WEST ? Tile.WEST_MASK : 0;

            door.DoorPosition = 0.5f;// b.HasDoors(mask) ? Random.Range(0.25f, 0.75f) : 0.5f;
            door.DoorSize = 0.25f;// b.HasDoors(mask) ? Random.Range(0.25f, 1.0f) : b.HasNeighbours(mask) ? 1.0f : 0.0f;
            door.GenerateMesh(tileSize, new Vector3(doorPosition.x, 0, doorPosition.y) * tileSize, transform);
        }
    }

    private List<Tile> FindNeighbours(Vector2 tilePosition)
    {
        List<Tile> neighbours = new List<Tile>();

        Vector2[] directions = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };

        foreach(Vector2 direction in directions)
        {
            Tile neighbour;
            if (tiles.TryGetValue(tilePosition + direction, out neighbour))
                neighbours.Add(neighbour);
        }

        return neighbours;
    }

    private Vector2 GenerateRandomDirection(Vector2 from)
    {                
        List<Vector2> attempts = new List<Vector2>(){ new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
        do
        {
            int index = Random.Range(0, attempts.Count);

            Vector2 direction = attempts[index];
            attempts.RemoveAt(index);

            if (!tiles.ContainsKey(from + direction))
                return direction;
        }
        while(attempts.Count > 0);

        return Vector2.zero;
    }

    private void ClearTiles()
    {
        var existing = GameObject.FindGameObjectsWithTag("Tile");

        if(existing == null)
            return;


        Array.ForEach(existing, child => {
            EditorApplication.delayCall += ()=> { DestroyImmediate(child); };
        });
    }

    private void OnDrawGizmos()
    {
        if (startTile == null || endTile == null)
            return;

        Vector3 north = new Vector3(0, 0, 1);
        Vector3 south = new Vector3(0, 0, -1);
        Vector3 east = new Vector3(1, 0, 0);
        Vector3 west = new Vector3(-1, 0, 0);

        foreach(Tile tile in tiles.Values)
        {

            

            Vector3 tilePos = new Vector3(tile.Position.x, 0, tile.Position.y);

            Gizmos.color = tile.IsMainBranch ? Color.blue : Color.yellow;

            Gizmos.DrawSphere(transform.position + (tilePos * tileSize), 0.5f);

            Gizmos.color = Color.blue;

            if (tile.HasNeighbours(Tile.NORTH_MASK))
                Gizmos.DrawLine(transform.position + (tilePos * tileSize), transform.position + ((tilePos + (north * 0.25f)) * tileSize));

            if (tile.HasNeighbours(Tile.SOUTH_MASK))
                Gizmos.DrawLine(transform.position + (tilePos * tileSize), transform.position + ((tilePos + (south * 0.25f)) * tileSize));

            if (tile.HasNeighbours(Tile.EAST_MASK))
                Gizmos.DrawLine(transform.position + (tilePos * tileSize), transform.position + ((tilePos + (east * 0.25f)) * tileSize));

            if (tile.HasNeighbours(Tile.WEST_MASK))
                Gizmos.DrawLine(transform.position + (tilePos * tileSize), transform.position + ((tilePos + (west * 0.25f)) * tileSize));

            Gizmos.color = Color.cyan;

            if (tile.HasDoors(Tile.NORTH_MASK))
                Gizmos.DrawLine(transform.position + ((tilePos + (north * 0.25f)) * tileSize), transform.position + ((tilePos + (north * 0.5f)) * tileSize));

            if (tile.HasDoors(Tile.SOUTH_MASK))
                Gizmos.DrawLine(transform.position + ((tilePos + (south * 0.25f)) * tileSize), transform.position + ((tilePos + (south * 0.5f)) * tileSize));

            if (tile.HasDoors(Tile.EAST_MASK))
                Gizmos.DrawLine(transform.position + ((tilePos + (east * 0.25f)) * tileSize), transform.position + ((tilePos + (east * 0.5f)) * tileSize));

            if (tile.HasDoors(Tile.WEST_MASK))
                Gizmos.DrawLine(transform.position + ((tilePos + (west * 0.25f)) * tileSize), transform.position + ((tilePos + (west * 0.5f)) * tileSize));

            Handles.Label(transform.position + (tilePos * tileSize), ""+tile.DistanceFromStart);
        }
                
        foreach (Wall door in doors)
        {
            Gizmos.color = Color.magenta;
            Vector2 doorPosition = (door.PositionA + ((door.PositionB - door.PositionA) * 0.5f));

            Gizmos.DrawSphere(transform.position + (new Vector3(doorPosition.x, 0, doorPosition.y) * tileSize), 0.25f);
     
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + (new Vector3(startTile.Position.x, 0, startTile.Position.y) * tileSize), 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (new Vector3(endTile.Position.x, 0, endTile.Position.y) * tileSize), 0.5f);

       
    }
}

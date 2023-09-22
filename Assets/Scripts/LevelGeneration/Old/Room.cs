using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



[ExecuteInEditMode]
public class Room : MonoBehaviour
{

    [SerializeField] public float TileSize = 5.0f;
    [SerializeField] private RoomTile[] roomTiles;

    [SerializeField] private Vector2Int check;

    private void Update() // Should be onStart later
    {
        roomTiles = gameObject.GetComponentsInChildren<RoomTile>();

    }

    public Tile[] Tiles
    {
        get 
        {
            Tile[] tiles = new Tile[roomTiles.Length];
            for (int i = 0; i< roomTiles.Length; ++i) 
            {
                tiles[i] = roomTiles[i].Tile;
            }
            return tiles;
        }
    }
    public List<RoomTile> FindEdgeTiles()
    {
        List<RoomTile> result = new List<RoomTile>();

        foreach(RoomTile roomTile in roomTiles)
        {
            if (roomTile.IsEdge)
                result.Add(roomTile);
        }
        
        return result;
    }

    public RoomTile FindClosestTile(Vector2 from)
    {
        RoomTile result = null;
        float minDistance = float.MaxValue;
        foreach(RoomTile roomTile in roomTiles)
        {
            if(result == null)
                result = roomTile;

            float distance = (from - roomTile.Tile.Position).magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                result = roomTile;
            }
        }

        return result;
    }

    public void OnDrawGizmos()
    {
        RoomTile closest = FindClosestTile(check);

        Gizmos.DrawCube(transform.position + (new Vector3(closest.Position.x, 0, closest.Position.y) * TileSize), new Vector3(TileSize, 0.5f, TileSize));
    }



}

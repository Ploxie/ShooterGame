using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class RoomTile : MonoBehaviour
{

    [SerializeField] public Vector2Int Position;
    [SerializeField] public bool ConnectNorth = true;
    [SerializeField] public bool ConnectSouth = true;
    [SerializeField] public bool ConnectEast  = true;
    [SerializeField] public bool ConnectWest  = true;
    [SerializeField] public bool IsEdge = false;

    private float tileSize; // Only relevant for Gizmos

    public Tile Tile
    {
        get 
        {
            Tile tile = new Tile(Position, tileSize);
            int mask = 0;
            mask |= ConnectNorth ? Tile.NORTH_MASK : 0;
            mask |= ConnectSouth ? Tile.SOUTH_MASK : 0;
            mask |= ConnectEast ? Tile.EAST_MASK : 0;
            mask |= ConnectWest ? Tile.WEST_MASK : 0;
            tile.AddNeighbours(mask);
            return tile; 
        }
    }

    private void Update() // Editor Debug
    {
        Room room = gameObject.GetComponentInParent<Room>();
        if (room == null)
            return;

        tileSize = room.TileSize;

    }

    private void OnDrawGizmos()
    {
        Vector3 north = new Vector3(0, 0, 1);
        Vector3 south = new Vector3(0, 0, -1);
        Vector3 east = new Vector3(1, 0, 0);
        Vector3 west = new Vector3(-1, 0, 0);

        Vector3 tilePosition = new Vector3(Position.x, 0, Position.y);

        Gizmos.color = IsEdge ? Color.blue : Color.cyan;
        Gizmos.DrawSphere(transform.position + (tilePosition * tileSize), 0.5f);// new Vector3(tileSize, 0.25f, tileSize));
        Gizmos.color = Color.black;
        //Gizmos.DrawWireCube(transform.position + (tilePosition * tileSize), new Vector3(tileSize, 0.25f, tileSize));

        Gizmos.color = Color.red;
        if (!ConnectNorth)
            Gizmos.DrawLine(transform.position + ((tilePosition + (north * 0.5f)) * tileSize) - new Vector3(tileSize * 0.5f, 0, 0), transform.position + ((tilePosition + (north * 0.5f)) * tileSize) + new Vector3(tileSize * 0.5f, 0, 0));

        if (!ConnectSouth)
            Gizmos.DrawLine(transform.position + ((tilePosition + (south * 0.5f)) * tileSize) - new Vector3(tileSize * 0.5f, 0, 0), transform.position + ((tilePosition + (south * 0.5f)) * tileSize) + new Vector3(tileSize * 0.5f, 0, 0));

        if (!ConnectEast)
            Gizmos.DrawLine(transform.position + ((tilePosition + (east * 0.5f)) * tileSize) - new Vector3(0, 0, tileSize * 0.5f), transform.position + ((tilePosition + (east * 0.5f)) * tileSize) + new Vector3(0, 0, tileSize * 0.5f));

        if (!ConnectWest)
            Gizmos.DrawLine(transform.position + ((tilePosition + (west * 0.5f)) * tileSize) - new Vector3(0, 0, tileSize * 0.5f), transform.position + ((tilePosition + (west * 0.5f)) * tileSize) + new Vector3(0, 0, tileSize * 0.5f));
    }

}

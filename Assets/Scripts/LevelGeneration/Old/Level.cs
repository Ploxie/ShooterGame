using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class Level{

    [SerializeField] private float tileSize = 5.0f;

    private Dictionary<Vector2Int, TileData> tiles;

    public Level()
    {
        tiles = new Dictionary<Vector2Int, TileData>();
    }

    public float TileSize
    {
        get { return tileSize; }
        set { tileSize = value; }
    }

    public Dictionary<Vector2Int, TileData> Tiles
    {
        get { return tiles; }
        private set { tiles = value; }
    }

    public TileData StartTile
    {
        get;
        private set;
    }

    public TileData EndTile
    {
        get;
        private set;
    }

    public void AddTile(TileData tileData)
    {
        tiles.TryAdd(tileData.Position, tileData);

        if(StartTile == null)
            StartTile = tileData;

        EndTile = tileData;
    }

    public void AddRoom(Room room)
    {

    }

    public void ClearTiles()
    {
        tiles.Clear();
    }

}

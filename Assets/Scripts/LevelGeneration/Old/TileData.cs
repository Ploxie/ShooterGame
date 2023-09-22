using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TileData
{

    [SerializeField] private Vector2Int position;

    public TileData(Vector2Int position)
    {
        Position = position;
    }

    public TileData(Vector2Int position, TileData parent)
    {
        Position = position;
        Parent = parent;
    }

    public Vector2Int Position
    {
        get { return position; }
        set { position = value; }
    }

    public TileData Parent
    {
        get;
        set;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}   

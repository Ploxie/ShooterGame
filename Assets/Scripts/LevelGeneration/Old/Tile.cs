using UnityEngine;
using UnityEngine.UI;


public class Tile
{
    public static readonly int NORTH_MASK = 0x1;
    public static readonly int SOUTH_MASK = 0x2;
    public static readonly int EAST_MASK = 0x4;
    public static readonly int WEST_MASK = 0x8;

    public static readonly int NORTH_EAST_MASK = NORTH_MASK | EAST_MASK;
    public static readonly int NORTH_WEST_MASK = NORTH_MASK | WEST_MASK;
    public static readonly int SOUTH_EAST_MASK = SOUTH_MASK | EAST_MASK;
    public static readonly int SOUTH_WEST_MASK = SOUTH_MASK | WEST_MASK;

    public Tile(Vector2Int position, float tileSize)
    {
        Position = position;
        TileSize = tileSize;
    }

    public Vector2Int Position
    {
        get;
        set;
    }

    public Tile Parent // Represents the previous tile in the level generation
    {
        get;
        set;
    }

    public float TileSize
    {
        get;
        private set;
    }

    public int DistanceFromStart
    {
        get;
        set;
    }

    public bool IsMainBranch
    {
        get;
        set;
    }

    public int NeighbourMask
    {
        get;
        private set;
    }

    public int DoorMask
    {
        get;
        set;
    }

    public int NeighbourCount
    {
        get
        {
            return ((NeighbourMask & Tile.WEST_MASK) >> 0x3) + ((NeighbourMask & Tile.EAST_MASK) >> 0x2) + ((NeighbourMask & Tile.SOUTH_MASK) >> 0x1) + (NeighbourMask & Tile.NORTH_MASK);
        }
    }
    public bool IsCorner
    {
        get
        {
            return HasNeighbours(NORTH_EAST_MASK) || HasNeighbours(NORTH_WEST_MASK) || HasNeighbours(SOUTH_EAST_MASK) || HasNeighbours(SOUTH_WEST_MASK);
        }
    }

    public bool IsCorridor
    {
        get
        {
            return (HasNeighbours(NORTH_MASK | SOUTH_MASK) && !HasNeighbours(EAST_MASK) && !HasNeighbours(WEST_MASK)) || (HasNeighbours(EAST_MASK | WEST_MASK) && !HasNeighbours(NORTH_MASK) && !HasNeighbours(SOUTH_MASK));
        }
    }

    public void AddNeighbour(Tile neighbour)
    {
        Vector2 distance = neighbour.Position - Position;

        if (distance.x > 0)
            NeighbourMask |= EAST_MASK;
        else if (distance.x < 0)
            NeighbourMask |= WEST_MASK;
        else if (distance.y > 0)
            NeighbourMask |= NORTH_MASK;
        else if (distance.y < 0)
            NeighbourMask |= SOUTH_MASK;
    }

    public void AddNeighbours(int mask)
    {
        NeighbourMask |= mask;
    }


    public bool HasNeighbours(int neighbourMask)
    {
        return (NeighbourMask & neighbourMask) == neighbourMask;
    }

    public bool HasNeighbour(Vector2 neighbourPosition)
    {
        Vector2 distance = neighbourPosition - Position;
        if (distance.x > 0)
            return HasNeighbours(EAST_MASK);
        else if (distance.x < 0)
            return HasNeighbours(WEST_MASK);
        else if (distance.y > 0)
            return HasNeighbours(NORTH_MASK);
        else if (distance.y < 0)
            return HasNeighbours(SOUTH_MASK);

        return false;
    }

    public bool HasDoors(int doorMask)
    {
        return (DoorMask & doorMask) == doorMask;
    }

    
}

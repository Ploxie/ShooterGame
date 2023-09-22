using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class TileObject : MonoBehaviour
{

    [SerializeField] private Level level;
    [SerializeField] private TileData tile;

    private void OnValidate()
    {
        Debug.Log("VALIDATE TILE");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 tileWorldPosition = new Vector3(tile.Position.x, 0, tile.Position.y);

        Gizmos.DrawCube(tileWorldPosition, new Vector3(5.0f, 0.25f, 5.0f));
    }


}

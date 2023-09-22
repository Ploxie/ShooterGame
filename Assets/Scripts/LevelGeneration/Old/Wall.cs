using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wall
{
    public enum Cardinal { NORTH, SOUTH, EAST, WEST }; // Convert to mask?

    public float DoorPosition = 0.5f;
    public float DoorSize;

    public Cardinal Orientation;

    public Vector2 PositionA;
    public Vector2 PositionB;


    public Wall(Vector2 positionA, Vector2 positionB)
    {
        PositionA = positionA;
        PositionB = positionB;

        Vector2 difference = (positionA - positionB);

        if (difference.x > 0)
            Orientation = Cardinal.EAST;
        else if (difference.x < 0)
            Orientation = Cardinal.WEST;
        else if (difference.y > 0)
            Orientation = Cardinal.NORTH;
        else if (difference.y < 0)
            Orientation = Cardinal.SOUTH;
    }

    public Wall(Cardinal orientation)
    {
        Orientation = orientation;
    }

    public static Vector3 CalculateSize(float tileSize, Cardinal orientation)
    {

        float wallHeight = tileSize * 0.5f;

        Vector3 horizontalScale = new Vector3(tileSize, wallHeight, tileSize / 25.0f);
        Vector3 verticalScale = new Vector3(tileSize / 25.0f, wallHeight, tileSize);

        return (orientation == Cardinal.NORTH || orientation == Cardinal.SOUTH) ? horizontalScale : verticalScale;
    }

    public void GenerateMesh(float tileSize, Vector3 worldPosition, Transform parent)
    {
        if (DoorSize >= 1.0f)
            return;

        if (DoorPosition > 0.9f || DoorPosition < 0.1f)
            return;

        float wallHeight = tileSize * 0.5f;

        if (DoorSize <= 0.0f)
        {
            Vector3 horizontalScale = new Vector3(tileSize, wallHeight, tileSize / 25.0f);
            Vector3 verticalScale = new Vector3(tileSize / 25.0f, wallHeight, tileSize);

            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.localScale = (Orientation == Cardinal.NORTH || Orientation == Cardinal.SOUTH) ? horizontalScale : verticalScale;
            wall.transform.parent = parent;
            wall.transform.position = worldPosition + new Vector3(0.0f, wallHeight * 0.5f, 0);
            wall.tag = "Tile";
            wall.name = "Wall";
            wall.isStatic = true;
            return;
        }

        float halfSize = tileSize * 0.5f;
        float halfOpening = halfSize * DoorSize;

        float leftSize = (halfSize - halfOpening) * (DoorPosition * 2.0f);
        float rightSize = (halfSize - halfOpening) * ((1.0f - DoorPosition) * 2.0f);
        

        float leftWallX = ((-leftSize * 0.5f) - halfOpening) - ((halfSize - halfOpening) * (1.0f - DoorPosition)) + (leftSize * 0.5f);
        float rightWallX = (rightSize * 0.5f) + halfOpening + ((halfSize - halfOpening) * (DoorPosition)) - (rightSize * 0.5f);


        Vector3 leftPos = Vector3.zero;
        Vector3 rightPos = Vector3.zero;
        Vector3 leftScale = Vector3.zero;
        Vector3 rightScale = Vector3.zero;

        switch(Orientation)
        {
            case Cardinal.NORTH:
            case Cardinal.SOUTH:
                leftPos = new Vector3(leftWallX, wallHeight * 0.5f, 0);
                rightPos = new Vector3(rightWallX, wallHeight * 0.5f, 0);
                leftScale = new Vector3(leftSize, wallHeight, tileSize / 25.0f);
                rightScale = new Vector3(rightSize, wallHeight, tileSize / 25.0f);
                break;
            case Cardinal.EAST:
            case Cardinal.WEST:
                leftPos = new Vector3(0, wallHeight * 0.5f, leftWallX);
                rightPos = new Vector3(0, wallHeight * 0.5f, rightWallX);
                leftScale = new Vector3(tileSize / 25.0f, wallHeight, leftSize);
                rightScale = new Vector3(tileSize / 25.0f, wallHeight, rightSize);
                break;
        }

        /*switch (Orientation)
        {
            case Cardinal.NORTH:
                {
                    leftPos = new Vector3(leftWallX, wallHeight * 0.5f, tileSize * 0.5f);
                    rightPos = new Vector3(rightWallX, wallHeight * 0.5f, tileSize * 0.5f);
                    leftScale = new Vector3(leftSize, wallHeight, tileSize / 25.0f);
                    rightScale = new Vector3(rightSize, wallHeight, tileSize / 25.0f);


                    // This V
                    leftPos = new Vector3(leftWallX, wallHeight * 0.5f, 0);
                    rightPos = new Vector3(rightWallX, wallHeight * 0.5f, 0);
                    leftScale = new Vector3(leftSize, wallHeight, tileSize / 25.0f);
                    rightScale = new Vector3(rightSize, wallHeight, tileSize / 25.0f);
                }
                break;
            case Cardinal.SOUTH:
                {
                    leftPos = new Vector3(leftWallX, wallHeight * 0.5f, -tileSize * 0.5f);
                    rightPos = new Vector3(rightWallX, wallHeight * 0.5f, -tileSize * 0.5f);
                    leftScale = new Vector3(leftSize, wallHeight, tileSize / 25.0f);
                    rightScale = new Vector3(rightSize, wallHeight, tileSize / 25.0f);
                }
                break;
            case Cardinal.EAST:
                {
                    
                    leftPos = new Vector3(tileSize * 0.5f, wallHeight * 0.5f, leftWallX);
                    rightPos = new Vector3(tileSize * 0.5f, wallHeight * 0.5f, rightWallX);
                    leftScale = new Vector3(tileSize / 25.0f, wallHeight, leftSize);
                    rightScale = new Vector3(tileSize / 25.0f, wallHeight, rightSize);
                    
                    // This V
                    leftPos = new Vector3(0, wallHeight * 0.5f, leftWallX);
                    rightPos = new Vector3(0, wallHeight * 0.5f, rightWallX);
                    leftScale = new Vector3(tileSize / 25.0f, wallHeight, leftSize);
                    rightScale = new Vector3(tileSize / 25.0f, wallHeight, rightSize);
                }
                break;
            case Cardinal.WEST:
                {
                    leftPos = new Vector3(-tileSize * 0.5f, wallHeight * 0.5f, leftWallX);
                    rightPos = new Vector3(-tileSize * 0.5f, wallHeight * 0.5f, rightWallX);
                    leftScale = new Vector3(tileSize / 25.0f, wallHeight, leftSize);
                    rightScale = new Vector3(tileSize / 25.0f, wallHeight, rightSize);
                }
                break;
        }*/

        //leftWall.SetActive(DoorPosition <= 0.9f && DoorPosition >= 0.1f);
        //rightWall.SetActive(DoorPosition <= 0.9f && DoorPosition >= 0.1f);


        
        if(leftSize >= tileSize * 0.2f)
        {
            GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);

            leftWall.transform.localScale = leftScale;
            leftWall.transform.parent = parent;
            leftWall.transform.position = worldPosition + leftPos;

            leftWall.tag = "Tile";
            leftWall.name = "LeftWall";
            leftWall.isStatic = true;
        }

        if(rightSize >= tileSize * 0.2f)
        {
            GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);

            rightWall.transform.localScale = rightScale;
            rightWall.transform.parent = parent;
            rightWall.transform.position = worldPosition + rightPos;

            rightWall.tag = "Tile";
            rightWall.name = "RightWall";
            rightWall.isStatic = true;
        }
    }
}

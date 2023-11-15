using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public abstract class Node : ScriptableObject
    {

        [HideInInspector] public string Guid;
        [HideInInspector] public Vector2 GraphPosition;
        public List<Node> Children = new ();
        public Node Parent;

        [HideInInspector] public List<Vector2Int> triedDirections = new ();
        public Vector2Int Position;
        public Vector2Int Size;
        

        public bool TriedEveryDirection()
        {
            foreach(var direction in Direction2D.CARDINAL)
            {
                if (!triedDirections.Contains(direction))
                    return false;
            }

            return true;
        }

        public RoomNode GetParentRoom()
        {
            if (Parent == null)
                return null;

            RoomNode parentRoom = Parent as RoomNode;
            if (parentRoom != null)
                return parentRoom;

            return Parent.GetParentRoom();
        }

        public DoorNode GetDoorNode()
        {
            if (Parent == null)
                return null;

            DoorNode door = Parent as DoorNode;
            if (door != null)
                return door;

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public class RoomNode : Node
    {

        public RoomModule Module;
        public RoomModule GeneratedModule;
        public RoomModule CreatedModule;

        public bool IsEnd;
        public bool HasKey;
        public bool IsWaveRoom;
        public Key.KeyType Key;

        public bool Intersects(RoomNode other)
        {
            return (Position.x <= other.Position.x + other.Size.x) &&
                (Position.x + Size.x >= other.Position.x) &&
            (Position.y <= other.Position.y + other.Size.y) &&
            (Position.y + Size.y >= other.Position.y);
        }        

        
        public bool HasDoorTo(RoomNode other)
        {
            foreach(var child in Children)
            {
                if(child is DoorNode && child.Children.Contains(other))
                 return true;
            }

            return false;
        }
    }
}

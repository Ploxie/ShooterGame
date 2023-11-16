using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

namespace Assets.Scripts.LevelGeneration
{
    [CreateAssetMenu()]
    public class Level : ScriptableObject
    {

        public Node rootNode;

        public List<Node> nodes = new();

        public Node CreateNode(System.Type type)
        {
            Node node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.Guid = GUID.Generate().ToString();
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            parent.Children.Add(child);
        }

        public void RemoveChild(Node parent, Node child)
        {
            parent.Children.Remove(child);
        }

        public List<Node> GetChildren(Node parent)
        {
            return parent.Children;
        }

        public RoomNode GetStartRoom()
        {
            foreach(var node in nodes)
            {
                RoomNode roomNode = node as RoomNode;
                if (roomNode != null && roomNode.Parent == null)
                    return roomNode;
            }

            return null;
        }

        public RoomNode GetEndRoom()
        {
            foreach (var node in nodes)
            {
                RoomNode roomNode = node as RoomNode;
                if (roomNode != null && roomNode.IsEnd)
                    return roomNode;
            }

            return null;
        }

    }
}

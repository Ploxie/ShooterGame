using Assets.Scripts.LevelGeneration;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = Assets.Scripts.LevelGeneration.Node;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{

    public Node node;

    public Port input;
    public Port output;

    public Action<NodeView> OnNodeSelected;

    public NodeView(Node node)
    {
        this.node = node;
        
        RoomNode roomNode = node as RoomNode;
        if(roomNode != null)
        {
            this.title = roomNode.Module != null ? roomNode.Module.name : "Random Room";
        }

        this.viewDataKey = node.Guid;


        style.left = node.GraphPosition.x;
        style.top = node.GraphPosition.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        node.GraphPosition.x = newPos.xMin;
        node.GraphPosition.y = newPos.yMin;
    }

    private void CreateInputPorts()
    {
        input = InstantiatePort(UnityEditor.Experimental.GraphView.Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, UnityEditor.Experimental.GraphView.Port.Capacity.Single, typeof(bool));
        input.portName = "";

        inputContainer.Add(input);
    }

    private void CreateOutputPorts()
    {
        output = InstantiatePort(UnityEditor.Experimental.GraphView.Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, UnityEditor.Experimental.GraphView.Port.Capacity.Multi, typeof(bool));
        output.portName = "";
        

        outputContainer.Add(output);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if(OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

}

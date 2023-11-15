using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using Codice.Client.BaseCommands.BranchExplorer;
using UnityEditor;
using Assets.Scripts.LevelGeneration;
using Node = Assets.Scripts.LevelGeneration.Node;
using UnityEngine;

public class LevelGeneratorView : GraphView
{
    public new class UxmlFactory : UxmlFactory<LevelGeneratorView, GraphView.UxmlTraits> { }

    public Action<NodeView> OnNodeSelected;
    Level level;

    public LevelGeneratorView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LevelGeneratorEditor.uss");
        styleSheets.Add(styleSheet);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    internal NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.Guid) as NodeView;
    }

    internal void PopulateView(Level level)
    {
        this.level = level;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        level.nodes.ForEach(n => CreateNodeView(n));

        level.nodes.ForEach(n =>
        {
            var children = level.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parent = FindNodeView(n);
                NodeView child = FindNodeView(c);

                Edge edge = parent.output.ConnectTo(child.input);
                AddElement(edge);
            });
        });
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    level.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if(edge != null)
                {
                    NodeView parent = edge.output.node as NodeView;
                    NodeView child = edge.input.node as NodeView;
                    level.RemoveChild(parent.node, child.node);
                }
            });
        }

        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parent = edge.output.node as NodeView;
                NodeView child = edge.input.node as NodeView;
                child.node.Parent = parent.node;
                level.AddChild(parent.node, child.node);

            });
        }

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        {
            var types = TypeCache.GetTypesDerivedFrom<Node>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"{type.Name}", (a) => 
                {
                    CreateNode(type); 

                });
            }
        }
    }


    void CreateNode(System.Type type)
    {
        Node node = level.CreateNode(type);
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
        
    }

}

using Assets.Scripts.LevelGeneration;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGeneratorEditor : EditorWindow
{

    LevelGeneratorView levelView;
    InspectorView inspectorView;

    [MenuItem("LevelGeneratorEditor/Editor ...")]
    public static void OpenWindow()
    {
        LevelGeneratorEditor wnd = GetWindow<LevelGeneratorEditor>();
        wnd.titleContent = new GUIContent("LevelGeneratorEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/LevelGeneratorEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LevelGeneratorEditor.uss");
        root.styleSheets.Add(styleSheet);

        levelView = root.Q<LevelGeneratorView>();
        inspectorView = root.Q<InspectorView>();
        levelView.OnNodeSelected = OnNodeSelectionChanged;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        Level level = Selection.activeObject as Level;
        if(level)
        {
            levelView.PopulateView(level);
        }
    }

    private void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}

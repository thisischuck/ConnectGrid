using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class LevelEditor : EditorWindow
{
    [MenuItem("Levels/Editor")]
    public static void ShowMyEditor()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<LevelEditor>();
        wnd.titleContent = new GUIContent("My Custom Editor");
    }

    public void OnGUI()
    {
        VisualElement root = rootVisualElement;
        GUILayout.BeginArea(new Rect(0, 0, root.localBound.width, root.localBound.height));

        GUILayout.Box("Test");

        GUILayout.Space(100);

        GUILayout.BeginHorizontal();
        for (int i = 0; i < 4; i++)
        {
            GUILayout.BeginVertical();
            for (int j = 0; j < 4; j++)
            {
                GUILayout.Button("Pressed");
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MailSystem))]
public class InboxEditor : Editor
{
    Vector2 scroll = new Vector2(100, 100);

    private string scriptText = string.Empty;

    // position of scroll view
    private Vector2 scrollPos;
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This is a script that helps you to add email manually.", MessageType.Info, true);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameStaticObjectSpawner))]
public class GameStaticObjectSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var staticObjectSpawner = (GameStaticObjectSpawner)target;
        if(GUILayout.Button("TryProcess"))
        {
            staticObjectSpawner.ProcessGeneration();
        }
    }
}
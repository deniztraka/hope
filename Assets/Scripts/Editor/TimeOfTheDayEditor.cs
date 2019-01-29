using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TimeOfTheDay))]
public class TimeOfTheDayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var timeOfTheDay = (TimeOfTheDay)target;
        if(GUILayout.Button("Calculate"))
        {
            timeOfTheDay.CalculateTimeOfTheDay();
        }
    }
}
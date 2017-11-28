/////////////////////
/// Authored by: It3ration (Oct 01, 2014) on Unity Forums
/// http://answers.unity3d.com/answers/801283/view.html
////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ReadOnlyAttribute : PropertyAttribute
{

}


[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}


public class Test
{
    [ReadOnly]
    public string a;
    [ReadOnly]
    public int b;
    [ReadOnly]
    public Material c;
    [ReadOnly]
    public List<int> d = new List<int>();
}
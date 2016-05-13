using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Picodex.Vxcm.VolumeAddress))]
[CanEditMultipleObjects]
public class VolumeAddressEditor : Editor
{
    SerializedProperty vector;

    void OnEnable()
    {
        vector = serializedObject.FindProperty("vector");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(vector);
        serializedObject.ApplyModifiedProperties();
    }
}
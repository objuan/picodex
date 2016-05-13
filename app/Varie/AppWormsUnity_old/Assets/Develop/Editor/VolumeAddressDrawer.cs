using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(Picodex.Vxcm.VolumeAddress))]
//public class VolumeAddressDrawer : PropertyDrawer
//{
//    const int curveWidth = 50;
//    const float min = 0;
//    const float max = 100;
//    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//    {
//        SerializedProperty x = prop.FindPropertyRelative("x");
//        SerializedProperty y = prop.FindPropertyRelative("y");

//        // Draw scale
//        //EditorGUI.Slider(
//        //    new Rect(pos.x, pos.y, pos.width - curveWidth, pos.height),
//        //    scale, min, max, label);

//        //// Draw curve
//        //int indent = EditorGUI.indentLevel;
//        //EditorGUI.indentLevel = 0;
//        //EditorGUI.PropertyField(
//        //    new Rect(pos.width - curveWidth, pos.y, curveWidth, pos.height),
//        //    curve, GUIContent.none);
//        //EditorGUI.indentLevel = indent;
//    }
//}
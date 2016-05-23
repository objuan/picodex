using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using Picodex.Vxcm;

namespace Picodex
{
    [CustomPropertyDrawer(typeof(Vector3i))]
    class DFVolumeResolutionDrawer : PropertyDrawer
    {

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
          //  EditorGUILayout.BeginVertical();
           

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect amountRect = new Rect(position.x, position.y, 30, position.height);
            Rect unitRect = new Rect(position.x + 35, position.y, 50, position.height);
            Rect nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

         //   EditorGUI.IntSlider(position, property.FindPropertyRelative("x"), 32,200, GUIContent.none);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("x"), GUIContent.none);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("y"), GUIContent.none);
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("z"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

            if (GUILayout.Button("Test Build1"))
            {

            }

            // EditorGUILayout.EndVertical();

        }
    }
}

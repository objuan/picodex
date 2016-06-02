
using UnityEditor;

namespace Picodex
{
    class EditorGUIHelper
    {
        public static void ObjectFields(SerializedObject serializedObject)
        {
            SerializedProperty prop = serializedObject.GetIterator();

            EditorGUILayout.BeginVertical();
            while (prop.NextVisible(true))
            {
                EditorGUILayout.PropertyField(prop);
            }
            prop.Reset();
            EditorGUILayout.EndVertical();
        }
    }
}

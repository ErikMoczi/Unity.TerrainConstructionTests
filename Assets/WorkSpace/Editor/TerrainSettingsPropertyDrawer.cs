using UnityEditor;
using UnityEngine;
using WorkSpace.Settings;

namespace WorkSpace.Editor
{
    [CustomPropertyDrawer(typeof(TerrainSettings))]
    public class TerrainSettingsPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue != null)
            {
                var serializedObject = new SerializedObject(property.objectReferenceValue as TerrainSettings);
                var resolution = serializedObject.FindProperty("resolution");
                var chunkCount = serializedObject.FindProperty("chunkCount");
                var noiseSettings = serializedObject.FindProperty("noiseSettings");
                var scale = serializedObject.FindProperty("scale");

                if (resolution != null)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(resolution);
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                if (chunkCount != null)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(chunkCount);
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                if (noiseSettings != null)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(noiseSettings, true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                if (scale != null)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(scale);
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
            else
            {
                EditorGUI.ObjectField(position, property);
            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace WorkSpace.Editor
{
    [CustomEditor(typeof(TerrainProfiler))]
    public class TerrainProfilerEditor : UnityEditor.Editor
    {
        private TerrainProfiler _terrainProfiler;

        private void Awake()
        {
            _terrainProfiler = target as TerrainProfiler;
        }

        private void OnEnable()
        {
            Undo.undoRedoPerformed += RefreshCreator;
        }

        private void OnDisable()
        {
            // ReSharper disable once DelegateSubtraction
            Undo.undoRedoPerformed -= RefreshCreator;
        }

        private void RefreshCreator()
        {
            if (Application.isPlaying)
            {
                _terrainProfiler.Generate();
            }
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(_terrainProfiler.Working))
            {
                if (_terrainProfiler.AutoUpdate)
                {
                    EditorGUI.BeginChangeCheck();
                    DrawDefaultInspector();
                    if (EditorGUI.EndChangeCheck() & _terrainProfiler.AutoUpdate)
                    {
                        RefreshCreator();
                    }
                }
                else
                {
                    DrawDefaultInspector();
                }

                if (GUILayout.Button("Generate"))
                {
                    RefreshCreator();
                }

                if (GUILayout.Button("Clear"))
                {
                    if (Application.isPlaying)
                    {
                        _terrainProfiler.Clear();
                    }
                }
            }
        }
    }
}
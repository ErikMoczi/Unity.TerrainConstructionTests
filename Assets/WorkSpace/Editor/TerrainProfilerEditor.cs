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

        private void RefreshCreator()
        {
            if (Application.isPlaying)
            {
                _terrainProfiler.Generate();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Generate"))
            {
                RefreshCreator();
            }
        }
    }
}
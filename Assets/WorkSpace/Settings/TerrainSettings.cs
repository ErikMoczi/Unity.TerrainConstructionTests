using UnityEditor;
using UnityEngine;

namespace WorkSpace.Settings
{
    [CreateAssetMenu(menuName = "TerrainConstruct/Terrain Settings", fileName = nameof(TerrainSettings))]
    public sealed class TerrainSettings :
#if UNITY_EDITOR
        ScriptableSingleton<TerrainSettings>
#else
        ScriptableObject
#endif
        , ITerrainSettings
    {
        private const int MaxChunks = 100;
        private const int MaxResolution = 255;

        [SerializeField, Range(1, MaxResolution)]
        private int resolution = 128;

        [SerializeField, Range(1, MaxChunks)] private int chunkCount = 10;
#pragma warning disable 649
        [SerializeField] private ChunkObject chunkObject;
        [SerializeField] private NoiseSettings noiseSettings;
#pragma warning restore 649

        public int Resolution => resolution;
        public int ChunkCount => chunkCount;
        public ChunkObject ChunkObject => chunkObject;
        public NoiseSettings NoiseSettings => noiseSettings;
    }
}
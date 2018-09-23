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
        [SerializeField, Range(1, 255)] private int _resolution = 128;
        [SerializeField, Range(1, 10000)] private int _chunkCount = 10;
        [SerializeField] private ChunkObject _chunkObject;
        [SerializeField] private NoiseSettings _noiseSettings;

        public int Resolution => _resolution;
        public int ChunkCount => _chunkCount;
        public ChunkObject ChunkObject => _chunkObject;
        public NoiseSettings NoiseSettings => _noiseSettings;
    }
}
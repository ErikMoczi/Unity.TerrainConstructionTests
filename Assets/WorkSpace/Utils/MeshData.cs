using UnityEngine;
using WorkSpace.Settings;

namespace WorkSpace.Utils
{
    internal struct MeshData
    {
        public int Size { get; }
        public float StepSize { get; }
        public Vector2 Offset { get; }
        public NoiseSettings NoiseSettings { get; }

        public MeshData(int size, Vector2 offset, NoiseSettings noiseSettings)
        {
            Size = size;
            StepSize = 1f / size;
            Offset = offset;
            NoiseSettings = noiseSettings;
        }
    }
}
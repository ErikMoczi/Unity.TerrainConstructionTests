using System;
using UnityEngine;

namespace WorkSpace.Settings
{
    [Serializable]
    public struct NoiseSettings : INoiseSettings
    {
#pragma warning disable 649
        [SerializeField] private float _frequency;
        [SerializeField, Range(1, 8)] private int _octaves;
        [SerializeField, Range(1, 8)] private float _lacunarity;
        [SerializeField, Range(0f, 1f)] private float _persistence;
        [SerializeField, Range(0f, 1f)] private float _amplitude;
#pragma warning restore 649

        public float Frequency => _frequency;
        public int Octaves => _octaves;
        public float Lacunarity => _lacunarity;
        public float Persistence => _persistence;
        public float Amplitude => _amplitude;
    }
}
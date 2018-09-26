using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using WorkSpace.Generators;
using WorkSpace.Settings;
using Debug = UnityEngine.Debug;

namespace WorkSpace
{
    [RequireComponent(typeof(MainThreadDispatch))]
    public class TerrainProfiler : MonoBehaviour
    {
        [SerializeField] private bool _autoUpdate;
        [SerializeField] private TerrainCreatorType _terrainCreatorType;
        [SerializeField] private TerrainSettings _terrainSettings;

        public bool AutoUpdate => _autoUpdate;
        public bool Working { get; private set; }

        private TerrainCreator _currentCreator;
        private Stopwatch _stopwatch;

        public void Generate()
        {
            Working = true;

            _currentCreator?.CleanUp();

            _currentCreator = GenerateNew();
            _currentCreator.SetUp();

            _stopwatch = Stopwatch.StartNew();
            _currentCreator.Run();

            MainThreadDispatch.Instance().Enqueue(() => { Working = false; });

            StartCoroutine(FinishWork());
        }

        private TerrainCreator GenerateNew()
        {
            switch (_terrainCreatorType)
            {
                case TerrainCreatorType.Sequential:
                {
                    return new SequentialTerrainCreator(_terrainSettings);
                }
                case TerrainCreatorType.Parallel:
                {
                    return new ParallelTerrainCreator(_terrainSettings);
                }
                case TerrainCreatorType.UnityJob:
                {
                    return new UnityJobTerrainCreator(_terrainSettings);
                }
                default:
                {
                    throw new Exception(
                        $"Missing implementation of {nameof(TerrainCreator)} for {_terrainCreatorType}"
                    );
                }
            }
        }

        private IEnumerator FinishWork()
        {
            while (Working)
            {
                yield return null;
            }

            _stopwatch.Stop();
            Debug.Log("--------------");
            Debug.Log($"<--- {_terrainCreatorType} --->");
            Debug.Log($"Total work: {_stopwatch.Elapsed.TotalMilliseconds:0.00 ms}");
        }
    }
}
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
        [SerializeField] private TerrainCreatorType _terrainCreatorType;
        [SerializeField] private TerrainSettings _terrainSettings;

        private TerrainCreator _currentCreator;

        private bool _working;
        private Stopwatch _stopwatch;

        public void Generate()
        {
            _currentCreator?.CleanUp();
            _working = true;

            _currentCreator = GenerateNew();
            _currentCreator.SetUp();

            _stopwatch = Stopwatch.StartNew();
            _currentCreator.Run();

            MainThreadDispatch.Instance().Enqueue(() => { _working = false; });

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
            while (_working)
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
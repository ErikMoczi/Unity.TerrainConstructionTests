using Unity.PerformanceTesting;
using UnityEditor;
using UnityEngine;

namespace WorkSpace.Settings
{
    [CreateAssetMenu(menuName = "TerrainConstruct/Test Settings", fileName = nameof(TestSettings))]
    public sealed class TestSettings :
#if UNITY_EDITOR
        ScriptableSingleton<TestSettings>
#else
        ScriptableObject
#endif
        , ITestSettings
    {
        [SerializeField, Range(0, 1000)] private int _totalRuns = 10;
        [SerializeField, Range(0, 10)] private int _warmUpCount = 3;
        [SerializeField, Range(0, 10)] private int _resultsPrecision = 4;
        [SerializeField] private SampleUnit _sampleUnit = SampleUnit.Millisecond;

        public int TotalRuns => _totalRuns;
        public int WarmUpCount => _warmUpCount;
        public int ResultsPrecision => _resultsPrecision;
        public SampleUnit SampleUnit => _sampleUnit;
    }
}
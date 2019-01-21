using UnityEditor;
using UnityEngine;
using Unity.PerformanceTesting;

namespace TerrainPerformance.Utils
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
        [SerializeField, Range(0, 1000)] private int totalRuns = 10;
        [SerializeField, Range(0, 10)] private int warmUpCount = 3;
        [SerializeField, Range(0, 10)] private int resultsPrecision = 4;
        [SerializeField] private SampleUnit sampleUnit = SampleUnit.Millisecond;

        public int TotalRuns => totalRuns;
        public int WarmUpCount => warmUpCount;
        public int ResultsPrecision => resultsPrecision;
        public SampleUnit SampleUnit => sampleUnit;
    }
}
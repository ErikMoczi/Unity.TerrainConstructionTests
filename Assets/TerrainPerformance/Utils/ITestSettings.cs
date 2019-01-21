using Unity.PerformanceTesting;

namespace TerrainPerformance.Utils
{
    public interface ITestSettings
    {
        int TotalRuns { get; }
        int WarmUpCount { get; }
        int ResultsPrecision { get; }
        SampleUnit SampleUnit { get; }
    }
}
using Unity.PerformanceTesting;

namespace WorkSpace.Settings
{
    public interface ITestSettings
    {
        int TotalRuns { get; }
        int WarmUpCount { get; }
        int ResultsPrecision { get; }
        SampleUnit SampleUnit { get; }
    }
}
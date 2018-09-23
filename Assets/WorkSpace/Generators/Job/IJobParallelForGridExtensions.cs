using Unity.Jobs;

namespace WorkSpace.Generators.Job
{
    internal static class IJobParallelForGridExtensions
    {
        public static JobHandle Schedule<T>(this T jobData, int innerloopBatchCount, JobHandle dependsOn = default)
            where T : struct, IJobParallelForGrid
        {
            return jobData.Schedule(jobData.GridSize, innerloopBatchCount, dependsOn);
        }

        public static void Run<T>(this T jobData) where T : struct, IJobParallelForGrid
        {
            jobData.Run(jobData.GridSize);
        }
    }
}
using Unity.Jobs;

namespace WorkSpace.Generators.Job
{
    internal interface IJobParallelForGrid : IJobParallelFor
    {
        int GridSize { get; }
    }
}
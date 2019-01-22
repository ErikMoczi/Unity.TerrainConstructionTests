using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.SingleLongArray
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class IterateSingleLongArraySystem : IterateSystem
    {
        #region Job

        [BurstCompile]
        private struct Job : IJobParallelFor
        {
            public NativeArray<float3> Data;

            public void Execute(int index)
            {
                Data[index] = math.float3(1f);
            }
        }

        #endregion

        protected override void OnUpdate()
        {
            new Job
            {
                Data = CreateSingleLongArraySystem.Data,
            }.Schedule(CreateSingleLongArraySystem.Data.Length, SetUpData.BatchCountLongArray).Complete();
        }
    }
}
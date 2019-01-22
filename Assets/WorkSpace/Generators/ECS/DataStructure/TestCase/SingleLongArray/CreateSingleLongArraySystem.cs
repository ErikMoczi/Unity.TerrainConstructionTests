using Unity.Collections;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.SingleLongArray
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateSingleLongArraySystem : CreateSystem
    {
        public static NativeArray<float3> Data;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            Data = new NativeArray<float3>(
                SetUpData.TotalPoints * SetUpData.ChunkCount,
                Allocator.Persistent, NativeArrayOptions.UninitializedMemory
            );
        }

        protected override void OnDestroyManager()
        {
            base.OnDestroyManager();
            Data.Dispose();
        }
    }
}
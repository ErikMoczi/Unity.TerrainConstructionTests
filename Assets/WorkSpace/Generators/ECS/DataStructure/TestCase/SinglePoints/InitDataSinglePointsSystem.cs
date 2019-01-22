using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using WorkSpace.Generators.ECS.DataStructure.Measuring;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.SinglePoints
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class InitDataSinglePointsSystem : InitDataSystem
    {
        #region Job

        [BurstCompile]
        private struct Job : IJobParallelFor
        {
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> Chunks;
            public ArchetypeChunkBufferType<DataComponent> DataComponent;
            [ReadOnly] public int Size;

            public void Execute(int index)
            {
                var chunk = Chunks[index];
                var dataComponents = chunk.GetBufferAccessor(DataComponent);
                for (var i = 0; i < chunk.Count; i++)
                {
                    dataComponents[i].ResizeUninitialized(Size);
                }
            }
        }

        #endregion

        private ComponentGroup _group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _group = GetComponentGroup(typeof(DataComponent));
        }

        protected override void OnUpdate()
        {
            var chunks = _group.CreateArchetypeChunkArray(Allocator.TempJob);
            new Job
            {
                Chunks = chunks,
                DataComponent = GetArchetypeChunkBufferType<DataComponent>(),
                Size = SetUpData.ChunkCount,
            }.Schedule(chunks.Length, 1).Complete();
        }
    }
}
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.Caching
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class IterateCachingSystem : IterateSystem
    {
        #region Job

        [BurstCompile]
        private struct Job : IJobParallelFor
        {
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> Chunks;
            public ArchetypeChunkBufferType<DataComponent> DataComponent;

            public void Execute(int index)
            {
                var chunk = Chunks[index];
                var dataComponents = chunk.GetBufferAccessor(DataComponent);
                for (var i = 0; i < chunk.Count; i++)
                {
                    var dataComponent = dataComponents[i];
                    for (var j = 0; j < dataComponent.Length; j++)
                    {
                        dataComponent[j] = math.float3(1f);
                    }
                }
            }
        }

        #endregion

        private ComponentGroup _group;

        public IterateCachingSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

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
            }.Schedule(chunks.Length, 64).Complete();
        }
    }
}
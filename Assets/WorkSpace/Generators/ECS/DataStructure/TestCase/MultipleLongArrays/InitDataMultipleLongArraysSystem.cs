using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MultipleLongArrays
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class InitDataMultipleLongArraysSystem : InitDataSystem
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

        public InitDataMultipleLongArraysSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _group = GetComponentGroup(typeof(DataComponent));
        }

        protected override void OnUpdate()
        {
            var totalVertices = (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1);
            var chunks = _group.CreateArchetypeChunkArray(Allocator.TempJob);
            new Job
            {
                Chunks = chunks,
                DataComponent = GetArchetypeChunkBufferType<DataComponent>(),
                Size = totalVertices,
            }.Schedule(chunks.Length, 64).Complete();
        }
    }
}
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MassivePoints
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class InitDataMassivePointsSystem : InitDataSystem
    {
        #region Job

        [BurstCompile]
        private struct Job : IJobParallelFor
        {
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> Chunks;
            public ArchetypeChunkComponentType<DataComponent> DataComponent;

            public void Execute(int index)
            {
                var chunk = Chunks[index];
                var dataComponents = chunk.GetNativeArray(DataComponent);
                for (var i = 0; i < chunk.Count; i++)
                {
                    dataComponents[i] = math.float3(1f);
                }
            }
        }

        #endregion

        private ComponentGroup _group;

        public InitDataMassivePointsSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
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
                DataComponent = GetArchetypeChunkComponentType<DataComponent>(),
            }.Schedule(chunks.Length, 64).Complete();
        }
    }
}
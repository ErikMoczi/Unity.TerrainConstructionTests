using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MultipleJobs
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed unsafe class IterateMultipleJobsSystem : IterateSystem
    {
        #region Job

        [BurstCompile]
        private struct Job : IJobParallelFor
        {
            [NativeDisableUnsafePtrRestriction] public DataComponent* DataComponent;

            public void Execute(int index)
            {
                var dataComponent = DataComponent[index];
                dataComponent.Value = math.float3(1f);
                DataComponent[index] = dataComponent;
            }
        }

        #endregion

        private ComponentGroup _group;

        public IterateMultipleJobsSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
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
            var jobHandles = new NativeArray<JobHandle>(TerrainSettings.ChunkCount, Allocator.Persistent);
            var archetypeDataComponent = GetArchetypeChunkBufferType<DataComponent>();
            var counter = 0;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < chunks.Length; i++)
            {
                var chunk = chunks[i];
                var dataComponents = chunk.GetBufferAccessor(archetypeDataComponent);
                for (var j = 0; j < dataComponents.Length; j++)
                {
                    var dataComponent = dataComponents[j];
                    jobHandles[counter] = new Job
                    {
                        DataComponent = (DataComponent*) dataComponent.GetUnsafePtr(),
                    }.Schedule(dataComponent.Length, 64);
                    counter++;
                }
            }

            JobHandle.CompleteAll(jobHandles);
            jobHandles.Dispose();
            chunks.Dispose();
        }
    }
}
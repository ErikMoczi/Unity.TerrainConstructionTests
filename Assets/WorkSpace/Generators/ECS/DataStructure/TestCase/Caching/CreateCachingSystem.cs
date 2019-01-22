using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.Caching
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateCachingSystem : CreateSystem
    {
        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var extraSize = (int) math.ceil(
                SetUpData.TotalPoints / (float) SetUpData.ArraySizeCached
            );
            var entityArchetype = EntityManager.CreateArchetype(typeof(DataComponent));
            var entities = new NativeArray<Entity>(SetUpData.ChunkCount * extraSize, Allocator.Temp);
            EntityManager.CreateEntity(entityArchetype, entities);
        }
    }
}
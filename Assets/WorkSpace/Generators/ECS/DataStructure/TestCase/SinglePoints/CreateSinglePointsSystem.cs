using Unity.Collections;
using Unity.Entities;
using WorkSpace.Generators.ECS.DataStructure.Measuring;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.SinglePoints
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateSinglePointsSystem : CreateSystem
    {
        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var entityArchetype = EntityManager.CreateArchetype(typeof(DataComponent));
            var entities = new NativeArray<Entity>(SetUpData.TotalPoints, Allocator.Temp);
            EntityManager.CreateEntity(entityArchetype, entities);
        }
    }
}
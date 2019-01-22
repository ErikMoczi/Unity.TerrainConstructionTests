using Unity.Collections;
using Unity.Entities;
using WorkSpace.Generators.ECS.DataStructure.Measuring;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MassivePoints
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateMassivePointsSystem : CreateSystem
    {
        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var entityArchetype = EntityManager.CreateArchetype(typeof(DataComponent));
            var entities = new NativeArray<Entity>(SetUpData.ChunkCount * SetUpData.TotalPoints, Allocator.Temp);
            EntityManager.CreateEntity(entityArchetype, entities);
        }
    }
}
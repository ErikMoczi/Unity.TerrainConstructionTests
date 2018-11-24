using Unity.Collections;
using Unity.Entities;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.SinglePoints
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateSinglePointsSystem : CreateSystem
    {
        public CreateSinglePointsSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var size = (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1);
            var entityArchetype = EntityManager.CreateArchetype(typeof(DataComponent));
            var entities = new NativeArray<Entity>(size, Allocator.Temp);
            EntityManager.CreateEntity(entityArchetype, entities);
        }
    }
}
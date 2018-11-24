using Unity.Collections;
using Unity.Entities;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MultipleLongArrays
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateMultipleLongArraysSystem : CreateSystem
    {
        public CreateMultipleLongArraysSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var entityArchetype = EntityManager.CreateArchetype(typeof(DataComponent));
            var entities = new NativeArray<Entity>(TerrainSettings.ChunkCount, Allocator.Temp);
            EntityManager.CreateEntity(entityArchetype, entities);
        }
    }
}
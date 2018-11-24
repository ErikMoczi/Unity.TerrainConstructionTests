using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.Caching
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateCachingSystem : CreateSystem
    {
        public CreateCachingSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var extraSize = (int) math.ceil(
                (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1) / (float) Settings.ArraySizeCached
            );
            var entityArchetype = EntityManager.CreateArchetype(typeof(DataComponent));
            var entities = new NativeArray<Entity>(TerrainSettings.ChunkCount * extraSize, Allocator.Temp);
            EntityManager.CreateEntity(entityArchetype, entities);
        }
    }
}
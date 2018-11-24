using Unity.Collections;
using Unity.Entities;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MassivePoints
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateMassivePointsSystem : CreateSystem
    {
        public CreateMassivePointsSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var size = (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1);
            var entityArchetype = EntityManager.CreateArchetype(typeof(DataComponent));
            var entities = new NativeArray<Entity>(TerrainSettings.ChunkCount * size, Allocator.Temp);
            EntityManager.CreateEntity(entityArchetype, entities);
        }
    }
}
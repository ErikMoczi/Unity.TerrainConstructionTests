using Unity.Entities;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.Base
{
    [DisableAutoCreation]
    public abstract class BaseSystem : ComponentSystem
    {
        protected ITerrainSettings TerrainSettings { get; }

        // ReSharper disable once PublicConstructorInAbstractClass
        public BaseSystem(ITerrainSettings terrainSettings)
        {
            TerrainSettings = terrainSettings;
        }
    }
}
using WorkSpace.Generators.ECS.Base;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS
{
    public sealed class SimpleEcsTerrainCreator : EcsTerrainCreator
    {
        public SimpleEcsTerrainCreator(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void DefineSystems(IEcsSystemProxy system)
        {
            system.Init<TerrainSystem>(constructorArguments: TerrainSettings);
        }
    }
}
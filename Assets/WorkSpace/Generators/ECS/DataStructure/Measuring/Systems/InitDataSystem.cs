using WorkSpace.Generators.ECS.Base;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring.Systems
{
    public abstract class InitDataSystem : BaseSystem
    {
        // ReSharper disable once PublicConstructorInAbstractClass
        public InitDataSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }
    }
}
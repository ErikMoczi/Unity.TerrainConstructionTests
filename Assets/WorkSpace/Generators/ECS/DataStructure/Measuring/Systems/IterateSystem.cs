using WorkSpace.Generators.ECS.Base;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring.Systems
{
    public abstract class IterateSystem : BaseSystem
    {
        // ReSharper disable once PublicConstructorInAbstractClass
        public IterateSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }
    }
}
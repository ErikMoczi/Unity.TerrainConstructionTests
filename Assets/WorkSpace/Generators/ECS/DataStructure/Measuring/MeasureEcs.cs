using WorkSpace.Generators.ECS.Base;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring
{
    public abstract class MeasureEcs : EcsTerrainCreatorBase
    {
        // ReSharper disable once PublicConstructorInAbstractClass
        public MeasureEcs(ITerrainSettings terrainSettings) : base(terrainSettings, false)
        {
        }
    }
}
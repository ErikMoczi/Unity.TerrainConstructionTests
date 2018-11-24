using WorkSpace.Generators.ECS.Base;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring.Systems
{
    public abstract class CreateSystem : BaseSystem
    {
        // ReSharper disable once PublicConstructorInAbstractClass
        public CreateSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected sealed override void OnUpdate()
        {
        }
    }
}
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.SingleLongArray
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class InitDataSingleLongArraySystem : InitDataSystem
    {
        public InitDataSingleLongArraySystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void OnUpdate()
        {
        }
    }
}
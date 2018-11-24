using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring
{
    public sealed class MeasureCreateSystem<TCreateSystem> : MeasureEcs
        where TCreateSystem : CreateSystem
    {
        public MeasureCreateSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void DefineRunSystems(IEcsSystemProxy system)
        {
            base.DefineRunSystems(system);
            system.Init<TCreateSystem>(false, TerrainSettings);
        }
    }
}
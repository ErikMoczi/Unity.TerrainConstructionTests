using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring
{
    public sealed class MeasureInitDataSystem<TCreateSystem, TInitDataSystem> : MeasureEcs
        where TCreateSystem : CreateSystem
        where TInitDataSystem : InitDataSystem
    {
        public MeasureInitDataSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void DefineSetUpSystems(IEcsSystemProxy system)
        {
            base.DefineSetUpSystems(system);
            system.Init<TCreateSystem>(false);
        }

        protected override void DefineRunSystems(IEcsSystemProxy system)
        {
            base.DefineRunSystems(system);
            system.Init<TInitDataSystem>();
        }
    }
}
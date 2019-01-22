using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring
{
    public sealed class MeasureIterateSystem<TCreateSystem, TInitDataSystem, TIterateSystem> : MeasureEcs
        where TCreateSystem : CreateSystem
        where TInitDataSystem : InitDataSystem
        where TIterateSystem : IterateSystem
    {
        public MeasureIterateSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void DefineSetUpSystems(IEcsSystemProxy system)
        {
            base.DefineSetUpSystems(system);
            system.Init<TCreateSystem>(false);
            system.Init<TInitDataSystem>();
        }

        protected override void DefineRunSystems(IEcsSystemProxy system)
        {
            base.DefineRunSystems(system);
            system.Init<TIterateSystem>();
        }
    }
}
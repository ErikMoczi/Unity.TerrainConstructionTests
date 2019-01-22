using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.Measuring
{
    public sealed class MeasureAllSystems<TCreateSystem, TInitDataSystem, TIterateSystem> : MeasureEcs
        where TCreateSystem : CreateSystem
        where TInitDataSystem : InitDataSystem
        where TIterateSystem : IterateSystem
    {
        public MeasureAllSystems(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void DefineRunSystems(IEcsSystemProxy system)
        {
            base.DefineRunSystems(system);
            system.Init<TCreateSystem>(false);
            system.Init<TInitDataSystem>();
            system.Init<TIterateSystem>();
        }
    }
}
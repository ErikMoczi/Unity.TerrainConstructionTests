using Unity.Rendering;
using Unity.Transforms;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.Base
{
    public abstract class EcsTerrainCreator : EcsTerrainCreatorBase
    {
        protected EcsTerrainCreator(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected sealed override void DefineRunSystems(IEcsSystemProxy system)
        {
            base.DefineRunSystems(system);
            DefineSystems(system);
            system.Init<RenderMeshSystemV2>();
            system.Init<EndFrameTransformSystem>();
            system.Init<CreateMissingRenderBoundsFromMeshRenderer>();
            system.Init<RenderBoundsUpdateSystem>();
        }

        protected sealed override void DefineSetUpSystems(IEcsSystemProxy system)
        {
            base.DefineSetUpSystems(system);
        }

        protected abstract void DefineSystems(IEcsSystemProxy system);
    }
}
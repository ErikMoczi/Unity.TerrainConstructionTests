using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS
{
    public sealed class ECSTerrainCreator : TerrainCreator
    {
        private readonly List<ComponentSystemBase> _systems = new List<ComponentSystemBase>();

        public ECSTerrainCreator(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        public override void SetUp()
        {
            World.DisposeAllWorlds();
        }

        public override void CleanUp()
        {
            _systems.Clear();
            World.DisposeAllWorlds();
        }

        public override void Run()
        {
            InitECS();
            RunSystems();
        }

        private void InitECS()
        {
            var world = new World("Test World");
            World.Active = world;

            world.CreateManager<EntityManager>();

            InitSystem<TerrainSystem>();
            InitSystem<RenderingSystemBootstrap>();
            InitSystem<EndFrameTransformSystem>();
            InitSystem<MeshInstanceRendererSystem>();

            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.Active);
        }

        private void InitSystem<T>() where T : ComponentSystemBase
        {
            var system = World.Active.GetOrCreateManager<T>();
            system.Enabled = false;
            _systems.Add(system);
        }

        private void RunSystems()
        {
            for (var i = 0; i < _systems.Count; i++)
            {
                _systems[i].Enabled = true;
                _systems[i].Update();
                _systems[i].Enabled = false;
            }
        }
    }
}
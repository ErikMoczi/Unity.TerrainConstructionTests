using System.Collections.Generic;
using Unity.Entities;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.Base
{
    public abstract class EcsTerrainCreatorBase : TerrainCreator
    {
        protected interface IEcsSystemProxy
        {
            T Init<T>(bool runUpdate = true, params object[] constructorArguments) where T : ComponentSystemBase;
        }

        #region EcsSystemProxy

        private interface IInternalEcsSystemProxy : IEcsSystemProxy
        {
            void Clear();
            void Run();
        }

        private sealed class EcsSystemProxy : IInternalEcsSystemProxy
        {
            private readonly List<ComponentSystemBase> _systems = new List<ComponentSystemBase>();

            public T Init<T>(bool runUpdate = true, params object[] constructorArguments)
                where T : ComponentSystemBase
            {
                var system = World.Active.CreateManager<T>(constructorArguments);
                system.Enabled = false;
                if (runUpdate)
                {
                    _systems.Add(system);
                }

                return system;
            }

            public void Run()
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < _systems.Count; i++)
                {
                    _systems[i].Enabled = true;
                    _systems[i].Update();
                    _systems[i].Enabled = false;
                }
            }

            public void Clear()
            {
                _systems.Clear();
            }
        }

        #endregion

        // ReSharper disable once MemberCanBePrivate.Global
        public const string WorldName = "TestWorld";
        private readonly bool _measureEcsBootstrap;
        private readonly IInternalEcsSystemProxy _system = new EcsSystemProxy();

        protected EcsTerrainCreatorBase(ITerrainSettings terrainSettings, bool measureEcsBootstrap = true) : base(
            terrainSettings)
        {
            _measureEcsBootstrap = measureEcsBootstrap;
        }

        public sealed override void SetUp()
        {
            World.DisposeAllWorlds();
            if (!_measureEcsBootstrap)
            {
                CreateDefaultWorld();
                DefineSetUpSystems(_system);
                _system.Run();
                _system.Clear();
            }
        }

        public sealed override void CleanUp()
        {
            _system.Clear();
            World.DisposeAllWorlds();
        }

        public sealed override void Run()
        {
            InitEcs();
            _system.Run();

            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.Active);
        }

        private void InitEcs()
        {
            if (_measureEcsBootstrap)
            {
                CreateDefaultWorld();
            }

            DefineRunSystems(_system);
        }

        protected virtual void DefineSetUpSystems(IEcsSystemProxy system)
        {
        }

        protected virtual void DefineRunSystems(IEcsSystemProxy system)
        {
        }

        private static void CreateDefaultWorld()
        {
            var world = new World(WorldName);
            World.Active = world;
            world.CreateManager<EntityManager>();
        }
    }
}
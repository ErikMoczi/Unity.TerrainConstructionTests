using System.Collections;
using NUnit.Framework;
using TerrainPerformance.Utils;
using Unity.PerformanceTesting;
using UnityEngine;
using WorkSpace;
using WorkSpace.Generators;
using WorkSpace.Generators.ECS;
using WorkSpace.Generators.Mono;

namespace TerrainPerformance
{
    [TestFixture(typeof(SequentialTerrainCreator))]
    [TestFixture(typeof(ParallelTerrainCreator))]
    [TestFixture(typeof(UnityJobTerrainCreator))]
    [TestFixture(typeof(SimpleEcsTerrainCreator))]
    internal sealed class TerrainConstruction<TTerrainCreator> : BaseTestTerrain<TTerrainCreator>
        where TTerrainCreator : class, ITerrainCreator
    {
        private bool _working;

        protected override void SetUpAdditional()
        {
            base.SetUpAdditional();
            CreateMainThreadDispatch();
        }

        protected override IEnumerator MainWork(bool firstRun = false, bool measure = true)
        {
            _working = true;
            TerrainCreator.SetUp();
            if (measure)
            {
                using (Measure.Scope(new SampleGroupDefinition(
                    Common.DefinitionName(TerrainCreator.GetType().Name, firstRun ? Common.FirstKeyWord : string.Empty),
                    TestSettings.SampleUnit)))
                {
                    yield return RunStatement();
                }
            }
            else
            {
                yield return RunStatement();
            }

            TerrainCreator.CleanUp();
        }

        private void CreateMainThreadDispatch()
        {
            var mainThreadDispatch = Object.FindObjectOfType<MainThreadDispatch>();
            if (mainThreadDispatch == null)
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GameObject(nameof(MainThreadDispatch), typeof(MainThreadDispatch));
            }
        }

        private IEnumerator RunStatement()
        {
            TerrainCreator.Run();
            MainThreadDispatch.Instance().Enqueue(() => { _working = false; });
            yield return new WaitWhile(() => _working);
        }
    }
}
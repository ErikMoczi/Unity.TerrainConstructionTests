using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Unity.PerformanceTesting;
using UnityEngine;
using WorkSpace;
using WorkSpace.Generators;
using WorkSpace.Generators.Mono;
using WorkSpace.Settings;
using WorkSpace.Utils;
using Object = UnityEngine.Object;

namespace TerrainPerformance
{
    [Category("Performance")]
    [TestFixture(typeof(SequentialTerrainCreator))]
    [TestFixture(typeof(ParallelTerrainCreator))]
    [TestFixture(typeof(UnityJobTerrainCreator))]
    public class TerrainConstruction<T> where T : class, ITerrainCreator
    {
        private ITestSettings _testSettings;
        private ITerrainSettings _terrainSettings;

        private ITerrainCreator _terrainCreator;
        private bool _working;

        [PerformanceUnityTest]
        public IEnumerator TerrainConstruction_Test()
        {
            #region FirstRun

            yield return MainWork(true);

            #endregion

            #region WarmUp

            for (var i = 0; i < _testSettings.WarmUpCount; i++)
            {
                yield return MainWork(measure: false);
            }

            #endregion

            #region TestCase

            for (var i = 0; i < _testSettings.TotalRuns; i++)
            {
                yield return MainWork();
            }

            #endregion
        }

        [SetUp]
        public void SetUp()
        {
            _testSettings = ResourcesData.LoadTestSettings();
            _terrainSettings = ResourcesData.LoadTerrainSettings();
            CreateMainThreadDispatch();
            _terrainCreator = InitTerrainCreator();
        }

//        Problem with [UnityTest] - IEnumerator, missing result data in TestContext.CurrentTestExecutionContext.CurrentResult.Output
//        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentTestExecutionContext;
            if (Equals(context.CurrentResult.ResultState, ResultState.Success))
            {
                var data = Utils.ParsePerformanceTestData(context.CurrentResult.Output);
                var performanceTest = Utils.GetPerformanceTest(data);
                var testRunnerResults = ParsePerformanceTest(performanceTest);
                var json = Utils.CreateTestRunnerResultJson(testRunnerResults);
                TestContext.WriteLine(Utils.TerrainConstructionPrefix + json);
            }
        }

        private ITerrainCreator InitTerrainCreator()
        {
            return (ITerrainCreator) Activator.CreateInstance(typeof(T), _terrainSettings);
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

        private IEnumerator MainWork(bool firstRun = false, bool measure = true)
        {
            _working = true;
            _terrainCreator.SetUp();
            if (measure)
            {
                using (Measure.Scope(new SampleGroupDefinition(
                    Utils.DefinitionName(_terrainCreator.GetType().Name, firstRun ? Utils.FirstKeyWord : string.Empty),
                    _testSettings.SampleUnit)))
                {
                    yield return RunStatement();
                }
            }
            else
            {
                yield return RunStatement();
            }

            _terrainCreator.CleanUp();
        }

        private IEnumerator RunStatement()
        {
            _terrainCreator.Run();
            MainThreadDispatch.Instance().Enqueue(() => { _working = false; });
            yield return new WaitWhile(() => _working);
        }

        private TestRunnerResult[] ParsePerformanceTest(PerformanceTest performanceTest)
        {
            var results = new List<TestRunnerResult>();
            for (var i = 0; i < performanceTest.SampleGroups.Count / 2; i++)
            {
                var sampleGroup = performanceTest.SampleGroups.First(group =>
                    group.Definition.Name.Equals(Utils.DefinitionName(_terrainCreator.GetType().Name)));
                var firstSampleGroup = performanceTest.SampleGroups.First(group =>
                    group.Definition.Name.Equals(Utils.DefinitionName(_terrainCreator.GetType().Name,
                        Utils.FirstKeyWord)));
                results.Add(
                    new TestRunnerResult
                    {
                        BaseSetUp = new TestRunnerResult._Base
                        {
                            TestName = _terrainCreator.GetType().Name,
                            TotalRuns = _testSettings.TotalRuns
                        },
                        Terrain = new TestRunnerResult._Terrain
                        {
                            Resolution = _terrainSettings.Resolution,
                            ChunkCount = _terrainSettings.ChunkCount,
                            Frequency = _terrainSettings.NoiseSettings.Frequency,
                            Octaves = _terrainSettings.NoiseSettings.Octaves,
                            Lacunarity = _terrainSettings.NoiseSettings.Lacunarity,
                            Persistence = _terrainSettings.NoiseSettings.Persistence
                        },
                        TestResults = new TestRunnerResult._Results
                        {
                            First = Math.Round(firstSampleGroup.Min, _testSettings.ResultsPrecision),
                            Min = Math.Round(sampleGroup.Min, _testSettings.ResultsPrecision),
                            Max = Math.Round(sampleGroup.Max, _testSettings.ResultsPrecision),
                            Median = Math.Round(sampleGroup.Median, _testSettings.ResultsPrecision),
                            Average = Math.Round(sampleGroup.Average, _testSettings.ResultsPrecision),
                            StandardDeviation = Math.Round(sampleGroup.StandardDeviation,
                                _testSettings.ResultsPrecision)
                        }
                    }
                );
            }

            return results.ToArray();
        }
    }
}
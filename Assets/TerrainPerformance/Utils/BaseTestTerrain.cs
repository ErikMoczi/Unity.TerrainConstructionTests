using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;
using WorkSpace.Generators;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace TerrainPerformance.Utils
{
    [Category("Performance")]
    internal abstract class BaseTestTerrain<TTerrainCreator> where TTerrainCreator : class, ITerrainCreator
    {
        protected ITestSettings TestSettings { get; private set; }
        protected ITerrainSettings TerrainSettings { get; private set; }
        protected TTerrainCreator TerrainCreator { get; private set; }

        #region Runner

        [PerformanceUnityTest]
        public IEnumerator TerrainConstruction_Test()
        {
            #region FirstRun

            yield return MainWork(true);

            #endregion

            #region WarmUp

            for (var i = 0; i < TestSettings.WarmUpCount; i++)
            {
                yield return MainWork(measure: false);
            }

            #endregion

            #region TestCase

            for (var i = 0; i < TestSettings.TotalRuns; i++)
            {
                yield return MainWork();
            }

            #endregion
        }

        #endregion

        #region SetUp

//        PerformanceTest running only with IEnumerator SetUp/TearDown
//        return new PerformanceTestCommand(new EnumerableSetUpTearDownCommand(new SetUpTearDownCommand(new UnityLogCheckDelegatingCommand(new EnumerableTestMethodCommand((TestMethod)command.Test)))));
        [UnitySetUp]
        // ReSharper disable once UnusedMember.Global
        public IEnumerator SetUp()
        {
            TestSettings = Common.LoadTestSettings();
            TerrainSettings = ResourcesData.LoadTerrainSettings();
            TerrainCreator = InitTerrainCreator();
            SetUpAdditional();
            yield return null;
        }

        #endregion

        #region TearDown

//        Problem with [UnityTest] - IEnumerator, missing result data in TestContext.CurrentTestExecutionContext.CurrentResult.Output
//        [UnityTearDown]
        // ReSharper disable once UnusedMember.Global
        public IEnumerator TearDown()
        {
            var context = TestContext.CurrentTestExecutionContext;
            if (Equals(context.CurrentResult.ResultState, ResultState.Success))
            {
//                var data = Utils.ParsePerformanceTestData(context.CurrentResult.Output);
                var data = Common.ParsePerformanceTestData(Resources.Load<TextAsset>("data").text);
                var performanceTest = Common.GetPerformanceTest(data);
                var testRunnerResults = ParsePerformanceTest(performanceTest);
                var json = Common.CreateTestRunnerResultJson(testRunnerResults);
                TestContext.WriteLine(Common.TerrainTestPrefix + json);
            }

            yield return null;
        }

        #endregion

        #region Helpers

        protected abstract IEnumerator MainWork(bool firstRun = false, bool measure = true);

        private TTerrainCreator InitTerrainCreator()
        {
            return (TTerrainCreator) Activator.CreateInstance(typeof(TTerrainCreator), TerrainSettings);
        }

        protected virtual void SetUpAdditional()
        {
        }

        private TestRunnerResult[] ParsePerformanceTest(PerformanceTest performanceTest)
        {
            var results = new List<TestRunnerResult>();
            for (var i = 0; i < performanceTest.SampleGroups.Count / 2; i++)
            {
                var sampleGroup = performanceTest.SampleGroups.First(group =>
                    group.Definition.Name.Equals(Common.DefinitionName(TerrainCreator.GetType().Name)));
                var firstSampleGroup = performanceTest.SampleGroups.First(group =>
                    group.Definition.Name.Equals(Common.DefinitionName(TerrainCreator.GetType().Name,
                        Common.FirstKeyWord)));
                results.Add(
                    new TestRunnerResult
                    {
                        BaseSetUp = new TestRunnerResult._Base
                        {
                            TestName = TerrainCreator.GetType().Name,
                            TotalRuns = TestSettings.TotalRuns
                        },
                        Terrain = new TestRunnerResult._Terrain
                        {
                            Resolution = TerrainSettings.Resolution,
                            ChunkCount = TerrainSettings.ChunkCount,
                            Frequency = TerrainSettings.NoiseSettings.Frequency,
                            Octaves = TerrainSettings.NoiseSettings.Octaves,
                            Lacunarity = TerrainSettings.NoiseSettings.Lacunarity,
                            Persistence = TerrainSettings.NoiseSettings.Persistence
                        },
                        TestResults = new TestRunnerResult._Results
                        {
                            First = Math.Round(firstSampleGroup.Min, TestSettings.ResultsPrecision),
                            Min = Math.Round(sampleGroup.Min, TestSettings.ResultsPrecision),
                            Max = Math.Round(sampleGroup.Max, TestSettings.ResultsPrecision),
                            Median = Math.Round(sampleGroup.Median, TestSettings.ResultsPrecision),
                            Average = Math.Round(sampleGroup.Average, TestSettings.ResultsPrecision),
                            StandardDeviation = Math.Round(sampleGroup.StandardDeviation,
                                TestSettings.ResultsPrecision)
                        }
                    }
                );
            }

            return results.ToArray();
        }

        #endregion
    }
}
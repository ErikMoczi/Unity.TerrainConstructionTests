using System.Collections;
using NUnit.Framework;
using TerrainPerformance.Utils;
using Unity.PerformanceTesting;
using WorkSpace.Generators;
using WorkSpace.Generators.ECS.DataStructure.Measuring;
using WorkSpace.Generators.ECS.DataStructure.TestCase.Caching;
using WorkSpace.Generators.ECS.DataStructure.TestCase.MassivePoints;
using WorkSpace.Generators.ECS.DataStructure.TestCase.MultipleJobs;
using WorkSpace.Generators.ECS.DataStructure.TestCase.MultipleLongArrays;
using WorkSpace.Generators.ECS.DataStructure.TestCase.SingleLongArray;
using WorkSpace.Generators.ECS.DataStructure.TestCase.SinglePoints;

namespace TerrainPerformance
{
    #region TestFixture

    #region MultipleLongArrays

    [TestFixture(
        typeof(MeasureAllSystems<CreateMultipleLongArraysSystem, InitDataMultipleLongArraysSystem,
            IterateMultipleLongArraysSystem>))]
    [TestFixture(typeof(MeasureCreateSystem<CreateMultipleLongArraysSystem>))]
    [TestFixture(typeof(MeasureInitDataSystem<CreateMultipleLongArraysSystem, InitDataMultipleLongArraysSystem>))]
    [TestFixture(
        typeof(MeasureIterateSystem<CreateMultipleLongArraysSystem, InitDataMultipleLongArraysSystem,
            IterateMultipleLongArraysSystem>))]

    #endregion

    #region MultipleJobs

    [TestFixture(
        typeof(MeasureAllSystems<CreateMultipleJobsSystem, InitDataMultipleJobsSystem, IterateMultipleJobsSystem>))]
    [TestFixture(typeof(MeasureCreateSystem<CreateMultipleJobsSystem>))]
    [TestFixture(typeof(MeasureInitDataSystem<CreateMultipleJobsSystem, InitDataMultipleJobsSystem>))]
    [TestFixture(
        typeof(MeasureIterateSystem<CreateMultipleJobsSystem, InitDataMultipleJobsSystem, IterateMultipleJobsSystem>))]

    #endregion

    #region Caching

    [TestFixture(typeof(MeasureAllSystems<CreateCachingSystem, InitDataCachingSystem, IterateCachingSystem>))]
    [TestFixture(typeof(MeasureCreateSystem<CreateCachingSystem>))]
    [TestFixture(typeof(MeasureInitDataSystem<CreateCachingSystem, InitDataCachingSystem>))]
    [TestFixture(typeof(MeasureIterateSystem<CreateCachingSystem, InitDataCachingSystem, IterateCachingSystem>))]

    #endregion

    #region MassivePoints

    [TestFixture(
        typeof(MeasureAllSystems<CreateMassivePointsSystem, InitDataMassivePointsSystem, IterateMassivePointsSystem>))]
    [TestFixture(typeof(MeasureCreateSystem<CreateMassivePointsSystem>))]
    [TestFixture(typeof(MeasureInitDataSystem<CreateMassivePointsSystem, InitDataMassivePointsSystem>))]
    [TestFixture(
        typeof(MeasureIterateSystem<CreateMassivePointsSystem, InitDataMassivePointsSystem, IterateMassivePointsSystem
        >))]

    #endregion

    #region SinglePoints

    [TestFixture(
        typeof(MeasureAllSystems<CreateSinglePointsSystem, InitDataSinglePointsSystem, IterateSinglePointsSystem>))]
    [TestFixture(typeof(MeasureCreateSystem<CreateSinglePointsSystem>))]
    [TestFixture(typeof(MeasureInitDataSystem<CreateSinglePointsSystem, InitDataSinglePointsSystem>))]
    [TestFixture(
        typeof(MeasureIterateSystem<CreateSinglePointsSystem, InitDataSinglePointsSystem, IterateSinglePointsSystem>))]

    #endregion

    #region SingleLongArray

    [TestFixture(
        typeof(MeasureAllSystems<CreateSingleLongArraySystem, InitDataSingleLongArraySystem,
            IterateSingleLongArraySystem>))]
    [TestFixture(typeof(MeasureCreateSystem<CreateSingleLongArraySystem>))]
    [TestFixture(typeof(MeasureInitDataSystem<CreateSingleLongArraySystem, InitDataSingleLongArraySystem>))]
    [TestFixture(
        typeof(MeasureIterateSystem<CreateSingleLongArraySystem, InitDataSingleLongArraySystem,
            IterateSingleLongArraySystem>))]

    #endregion

    #endregion

    internal sealed class DataStructureMeasure<TTerrainCreator> : BaseTestTerrain<TTerrainCreator>
        where TTerrainCreator : class, ITerrainCreator
    {
        protected override IEnumerator MainWork(bool firstRun = false, bool measure = true)
        {
            TerrainCreator.SetUp();
            if (measure)
            {
                using (Measure.Scope(new SampleGroupDefinition(
                    Common.DefinitionName(TerrainCreator.GetType().Name, firstRun ? Common.FirstKeyWord : string.Empty),
                    TestSettings.SampleUnit)))
                {
                    TerrainCreator.Run();
                }
            }
            else
            {
                TerrainCreator.Run();
            }

            yield return null;
            TerrainCreator.CleanUp();
        }
    }
}
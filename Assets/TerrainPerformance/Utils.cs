using System;
using System.Text.RegularExpressions;
using Unity.PerformanceTesting;
using UnityEngine;

namespace TerrainPerformance
{
    internal static class Utils
    {
        public const string FirstKeyWord = "_First";
        public const string DefinitionPrefix = "#";
        public const string PerformanceTestPrefix = "##performancetestresult:";
        public const string TerrainConstructionPrefix = "##terrainconstructionresult:";

        public static string DefinitionName(string name, string suffix = "")
        {
            return $"{DefinitionPrefix}{name}{suffix}";
        }

        public static string ParsePerformanceTestData(string data)
        {
            var performanceData = Regex.Match(data, $@"{PerformanceTestPrefix}[^\n]+$");
            if (performanceData.Success & performanceData.Groups.Count == 1)
            {
                return performanceData.Groups[0].Value.Trim().Replace(PerformanceTestPrefix, string.Empty);
            }

            throw new Exception(
                $"Problem with parsing performance test data, check Unity API {typeof(PerformanceTest)}"
            );
        }

        public static PerformanceTest GetPerformanceTest(string jsonData)
        {
            return JsonUtility.FromJson<PerformanceTest>(jsonData);
        }

        public static string CreateTestRunnerResultJson(TestRunnerResult[] testRunnerResults)
        {
            var data = new Wrapper<TestRunnerResult> {data = testRunnerResults};
            return JsonUtility.ToJson(data);
        }
    }
}
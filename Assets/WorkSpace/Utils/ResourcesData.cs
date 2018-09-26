using UnityEngine;
using WorkSpace.Settings;

namespace WorkSpace.Utils
{
    public static class ResourcesData
    {
        private const string TestSettingsAsset = "TestSettings";
        private const string TerrainSettingsAsset = "TerrainSettings";

        public static ITestSettings LoadTestSettings()
        {
            return Resources.Load<TestSettings>(TestSettingsAsset);
        }

        public static ITerrainSettings LoadTerrainSettings()
        {
            return Resources.Load<TerrainSettings>(TerrainSettingsAsset);
        }
    }
}
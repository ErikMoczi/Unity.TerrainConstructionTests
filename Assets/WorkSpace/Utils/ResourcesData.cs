using UnityEngine;
using WorkSpace.Settings;

namespace WorkSpace.Utils
{
    public static class ResourcesData
    {
        private const string TerrainSettingsAsset = "TerrainSettings";

        public static ITerrainSettings LoadTerrainSettings()
        {
            return Resources.Load<TerrainSettings>(TerrainSettingsAsset);
        }
    }
}
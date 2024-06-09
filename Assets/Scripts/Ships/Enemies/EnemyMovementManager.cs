using System.Collections.Generic;
using UnityEngine;

public static class EnemyMovementManager
{
    private static Dictionary<string, Dictionary<int, int>> lastUsedSpawnIndex = new Dictionary<string, Dictionary<int, int>>();

    public static DeterminedPath GetPathData(string shipType, int pathIndex, string presetPath = null)
    {
        PathData pathData;
        List<int> spawnList;
        int spawnIndex;
        List<PathData> pathList;
        string indexKey;

        if (!string.IsNullOrEmpty(presetPath) && GameConfig.EnemyPathPresets.TryGetValue(presetPath, out var presetPathData))
        {
            pathData = presetPathData;
            indexKey = presetPath;
        }
        else
        {
            switch (shipType) // ! Refactor to name field to prevent adding to switch case, although there should be default behaviour...
            {
                case "SF1":
                    pathList = GameConfig.EnemyPaths.SF1;
                    break;
                case "SF2":
                    pathList = GameConfig.EnemyPaths.SF2;
                    break;
                default:
                    Debug.LogError($"No path data found for ship type: {shipType}");
                    return null;
            }

            if (pathList == null || pathList.Count == 0)
            {
                Debug.LogError($"Path list is empty for ship type: {shipType}");
                return null;
            }

            pathData = pathList[pathIndex];
            indexKey = shipType;
        }

        spawnList = pathData.spawns;

        if (!lastUsedSpawnIndex.ContainsKey(indexKey))
        {
            lastUsedSpawnIndex[indexKey] = new Dictionary<int, int>();
        }

        if (!lastUsedSpawnIndex[indexKey].ContainsKey(pathIndex))
        {
            lastUsedSpawnIndex[indexKey][pathIndex] = -1;
        }

        int lastIndex = lastUsedSpawnIndex[indexKey][pathIndex];
        int nextIndex = (lastIndex + 1) % spawnList.Count;

        lastUsedSpawnIndex[indexKey][pathIndex] = nextIndex;

        spawnIndex = spawnList[nextIndex];

        return new DeterminedPath
        {
            spawnIndex = spawnIndex,
            pathData = pathData
        };
    }
}

using System.Collections.Generic;
using UnityEngine;

public static class EnemyMovementManager
{
    private static Dictionary<string, int> LastUsedSpawnIndexes = new Dictionary<string, int>();

    public static DeterminedPath GetPathData(string shipType, string pathPreset = null)
    {
        PathData pathData;
        int spawnIndex;

        // If null, set to...
        pathPreset ??= FetchValidPathPreset(shipType);

        if (GameConfig.EnemyPathPresets.TryGetValue(pathPreset, out var pathPresetData))
        {
            pathData = pathPresetData;
        }
        else
        {
            Debug.LogError($"Invalid PathPreset ${pathPreset}");
            return null;
        }

        spawnIndex = DetermineSpawnIndex(pathPreset, pathData.spawns);

        return new DeterminedPath
        {
            spawnIndex = spawnIndex,
            pathData = pathData
        };
    }

    public static string FetchValidPathPreset(string shipType)
    {
        List<string> pathList;

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

        return pathList[Random.Range(0, pathList.Count)];
    }

    private static int DetermineSpawnIndex(string pathPreset, List<int> spawnList)
    {
        if (!LastUsedSpawnIndexes.ContainsKey(pathPreset))
        {
            LastUsedSpawnIndexes[pathPreset] = 0;
        }

        int lastIndex = LastUsedSpawnIndexes[pathPreset];
        int nextIndex = (lastIndex + 1) % spawnList.Count;

        LastUsedSpawnIndexes[pathPreset] = nextIndex;

        int spawnIndex = spawnList[nextIndex];
        return spawnIndex;
    }
}

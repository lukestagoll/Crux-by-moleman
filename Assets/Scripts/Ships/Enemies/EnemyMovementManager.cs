using System.Collections.Generic;
using UnityEngine;

public static class EnemyMovementManager
{
    private static Dictionary<string, Dictionary<int, int>> lastUsedSpawnIndex = new Dictionary<string, Dictionary<int, int>>();

    public static PathData GetPathData(string shipType, int pathIndex)
    {
        List<PathData> pathList;

        switch (shipType)
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

        return pathList[pathIndex];
    }

    public static int GetSpawnIndex(string shipType, int pathIndex)
    {
        List<int> spawnList;

        switch (shipType)
        {
            case "SF1":
                spawnList = GameConfig.EnemyPaths.SF1[pathIndex].spawns;
                break;
            case "SF2":
                spawnList = GameConfig.EnemyPaths.SF2[pathIndex].spawns;
                break;
            default:
                Debug.LogError($"No spawn data found for ship type: {shipType}");
                return -1;
        }

        if (spawnList == null || spawnList.Count == 0)
        {
            Debug.LogError($"Spawn list is empty for ship type: {shipType}");
            return -1;
        }

        if (!lastUsedSpawnIndex.ContainsKey(shipType))
        {
            lastUsedSpawnIndex[shipType] = new Dictionary<int, int>();
        }

        if (!lastUsedSpawnIndex[shipType].ContainsKey(pathIndex))
        {
            lastUsedSpawnIndex[shipType][pathIndex] = -1;
        }

        int lastIndex = lastUsedSpawnIndex[shipType][pathIndex];
        int nextIndex = (lastIndex + 1) % spawnList.Count;

        lastUsedSpawnIndex[shipType][pathIndex] = nextIndex;

        return spawnList[nextIndex];
    }
}

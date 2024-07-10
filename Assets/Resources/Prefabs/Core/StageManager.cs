using UnityEngine;

public static class StageManager
{
    public static void StartStage(int stageIndex)
    {
        Debug.Log($"Initializing Stage {stageIndex}");
        StageData stageData = GameConfig.GameData.Stages[stageIndex];
        if (!ValidateStage(stageData))
        {
            Debug.LogError("Stage validation failed. Stopping stage initialization.");
            EndStage();
        }
        LevelManager.StartLevels(stageData);
    }

    private static bool ValidateStage(StageData stage)
    {
        if (stage.Levels == null || stage.Levels.Length == 0) {
            Debug.LogError("No Levels found in Stage!");
            return false;
        }
        return true;
    }

    public static void HandleAllLevelsCompleted()
    {
        // Stage Complete Events
        EndStage();
    }

    private static void EndStage()
    {
        Debug.Log("Stage Completed!");
        GameManager.HandleStageCompleted();
    }
}
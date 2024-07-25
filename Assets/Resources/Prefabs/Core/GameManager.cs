using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static bool IsPaused = false;
    public static int Score { get; private set; }

    public static async void InitiateGameplay(bool skipLoad)
    {
        if (!skipLoad)
        {
            await LoadSceneAsync("Game");
        }

        Score = GameConfig.InitialScore;
        PlayerManager.Inst.Lives = GameConfig.InitialLives;
        PlayerManager.Inst.BuildInitialSkills();
        PlayerManager.Inst.SpawnPlayer();
        StageManager.StartStage(0);
    }

    public static void IncrementScore(int pointsToAdd)
    {
        Score += pointsToAdd;
        HUDManager.Inst.UpdateScoreDisplay();
    }

    public static void HandleGameOver()
    {
        Debug.Log("GAME OVER");
        SceneManager.LoadScene("MainMenu");
    }

    public static void HandleStageSelected(int stageIndex)
    {
        StageManager.StartStage(stageIndex);
    }

    public static void HandleStageCompleted()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private static Task LoadSceneAsync(string sceneName)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.completed += _ => tcs.SetResult(true);
        return tcs.Task;
    }
}

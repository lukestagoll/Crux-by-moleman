using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static bool IsPaused = false;
    public static int Score { get; private set; }
    public static bool SceneIsChanging;

    public static async void InitiateGameplay(bool skipLoad)
    {
        // If Game scene already loaded
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

    public static async void HandleGameOver()
    {
        Debug.Log("GAME OVER");
        await LoadSceneAsync("MainMenu");
    }

    public static void HandleStageSelected(int stageIndex)
    {
        StageManager.StartStage(stageIndex);
    }

    public static async void HandleStageCompleted()
    {
        await LoadSceneAsync("MainMenu");
    }

    public static Task LoadSceneAsync(string sceneName)
    {
        SceneIsChanging = true;
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.completed += _ => {
            tcs.SetResult(true);
            Time.timeScale = 1f;
            SceneIsChanging = false;
        };
        return tcs.Task;
    }

    public static void LoadScene(string sceneName)
    {
        SceneIsChanging = true;
        SceneManager.LoadScene(sceneName);
        SceneIsChanging = false;
    }
}

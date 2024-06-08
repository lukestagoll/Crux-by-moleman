using UnityEngine;
using UnityEngine.SceneManagement; // Add this namespace

public class GameLoader : MonoBehaviour
{
    private bool WasNotLoaded { get; set; }
    void Awake()
    {
        if (!GameConfig.HasBeenLoaded)
        {
          WasNotLoaded = true;
          GameConfig.Initialise();
        }
    }

    void Start()
    {
      if (WasNotLoaded)
      {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Game")
        {
          GameManager.InitiateGameplay(true);
        }
      }
    }
}

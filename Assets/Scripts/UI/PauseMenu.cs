using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Inst { get; private set; }
    public GameObject pauseMenuUI;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("PauseMenu already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;
        Resume();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.IsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.IsPaused = true;
    }

    public void OpenOptions()
    {
      // Assuming you have an options menu scene or a way to show options
      Debug.Log("Open options here.");
      // GameManager.NavigateToOptions();
      // Implement your options menu functionality here, e.g., SceneManager.LoadScene("OptionsMenu");
    }

    public void GoToMainMenu()
    {
      Resume();
      SceneManager.LoadScene("MainMenu");
    }

    // Add other methods for buttons like GoToMainMenu(), OpenOptions(), etc.
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // ! Can probably not inherit from MonoBehaviour
    // ! Setup an Instantiate function and then sceneLock the functionality
    // ! This allows the pauseMenu to be loaded and also not be active in the editor
    public static UIManager Inst { get; private set; }
    // private bool GameIsActive = false;
    public GameObject MainMenuUI;
    private GameObject ShipSelectionUI;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("UIManager already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;
    }

    void Start()
    {
        ShipSelectionUI = Instantiate(AssetManager.ShipSelectionUIPrefab);
        ShipSelectionUI.SetActive(false);
    }

    public void TransitionToShipSelection()
    {
        if (MainMenuUI == null) 
        {
            Debug.LogError("MainMenuUI is null");
            return;
        }
        MainMenuUI.SetActive(false);
        ShipSelectionUI.SetActive(true);
    }

    public void HandleMoveUp()
    {

    }

    public void HandleMoveDown()
    {

    }

    public void HandleMoveLeft()
    {
        // if (SceneManager.GetActiveScene().name == "MainMenu")
        if (ShipSelectionUI.activeSelf)
        {
            ShipSelectionUI.GetComponent<ShipSelection>().MoveCursorLeft();
        }
    }

    public void HandleMoveRight()
    {
        if (ShipSelectionUI.activeSelf)
        {
            ShipSelectionUI.GetComponent<ShipSelection>().MoveCursorRight();
        }
    }
}

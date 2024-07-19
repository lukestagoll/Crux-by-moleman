using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ! Can probably not inherit from MonoBehaviour
    // ! Setup an Instantiate function and then sceneLock the functionality
    // ! This allows the pauseMenu to be loaded and also not be active in the editor
    public static UIManager Inst { get; private set; }
    public bool GameIsActive = false;
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

    private void ActivateMainMenuUI()
    {

    }

    private void ActivateStageUI()
    {

    }

    private void ActivatePauseUI()
    {
        
    }

    private void DeactivateMainMenuUI()
    {

    }

    private void DeactivateStageUI()
    {

    }

    private void DeactivatePauseUI()
    {
        
    }
}

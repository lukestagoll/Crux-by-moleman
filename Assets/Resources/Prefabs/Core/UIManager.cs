using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst { get; private set; }
    public bool GameIsActive = false;

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

    /*
        Some things to consider:
            - MainMenu and Stage should never both be active
            - Pause cannot be activated when MainMenu is active
            - Pause can be activated with Stage
    */

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

using UnityEngine;

public class GameInputHandler : MonoBehaviour
{
    private GameControls controls;

    private void Awake()
    {
        controls = new GameControls();
        controls.Gameplay.Pause.performed += _ => TogglePause();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void TogglePause()
    {
        Debug.Log("TOGGLE PAUSE");
        if (GameManager.IsPaused)
            PauseMenu.Inst.Resume();
        else
            PauseMenu.Inst.Pause();
    }
}

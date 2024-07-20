using UnityEngine;

public class MainMenuInputHandler : MonoBehaviour
{
    private GameControls controls;

    private void Awake()
    {
        controls = new GameControls();
        controls.MainMenu.MoveUp.performed += _ => MoveUp();
        controls.MainMenu.MoveDown.performed += ctx => MoveDown();
        controls.MainMenu.MoveLeft.performed += ctx => MoveLeft();
        controls.MainMenu.MoveRight.performed += ctx => MoveRight();
        controls.MainMenu.Select.performed += ctx => Select();
    }

    private void OnEnable()
    {
        controls.MainMenu.Enable();
    }

    private void OnDisable()
    {
        controls.MainMenu.Disable();
    }

    private void MoveUp()
    {
        Debug.Log("[MainMenuInputHandler] MoveUp");
        UIManager.Inst.HandleMoveUp();
    }

    private void MoveDown()
    {
        Debug.Log("[MainMenuInputHandler] MoveDown");
        UIManager.Inst.HandleMoveDown();
    }

    private void MoveLeft()
    {
        Debug.Log("[MainMenuInputHandler] MoveLeft");
        UIManager.Inst.HandleMoveLeft();
    }

    private void MoveRight()
    {
        Debug.Log("[MainMenuInputHandler] MoveRight");
        UIManager.Inst.HandleMoveRight();
    }

    private void Select()
    {
        Debug.Log("[MainMenuInputHandler] Select");
        UIManager.Inst.HandleSelect();
    }
}

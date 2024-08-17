using UnityEngine;

public class GameInputHandler : MonoBehaviour
{
    public static GameInputHandler Inst { get; private set; }
    private GameControls controls;

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("GameInputHandler already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;

        controls = new GameControls();
        controls.Gameplay.Pause.performed += _ => TogglePause();
        controls.Gameplay.PrimaryAttack.performed += ctx => OnPrimaryAttackPerformed();
        controls.Gameplay.PrimaryAttack.canceled += ctx => OnPrimaryAttackCanceled();
        controls.Gameplay.SpecialAttack.performed += ctx => OnSpecialAttackPerformed();
        controls.Gameplay.SpecialAttack.canceled += ctx => OnSpecialAttackCanceled();

        controls.MenuNavigation.MoveUp.performed += ctx => MoveUp();
        controls.MenuNavigation.MoveDown.performed += ctx => MoveDown();
        controls.MenuNavigation.MoveLeft.performed += ctx => MoveLeft();
        controls.MenuNavigation.MoveRight.performed += ctx => MoveRight();
        controls.MenuNavigation.Select.performed += ctx => Select();
    }

    public void EnableGameplayControls()
    {
        controls.Gameplay.Enable();
    }

    public void DisableGameplayControls()
    {
        controls.Gameplay.Disable();
    }
    public void EnableMenuNavigationControls()
    {
        controls.MenuNavigation.Enable();
    }

    public void DisableMenuNavigationControls()
    {
        controls.MenuNavigation.Disable();
    }

    private void TogglePause()
    {
        GameManager.TogglePause();
    }

    private void OnPrimaryAttackPerformed()
    {
        PlayerShip playerShip = PlayerManager.Inst.ActivePlayerShip;
        if (playerShip != null) playerShip.EnablePrimaryFire();
    }

    private void OnPrimaryAttackCanceled()
    {
        PlayerShip playerShip = PlayerManager.Inst.ActivePlayerShip;
        if (playerShip != null) playerShip.DisablePrimaryFire();
    }

    private void OnSpecialAttackPerformed()
    {
        GameManager.DisableFirstSpecialWeaponUI();
        PlayerShip playerShip = PlayerManager.Inst.ActivePlayerShip;
        if (playerShip != null) playerShip.EnableSpecialFire();
    }

    private void OnSpecialAttackCanceled()
    {
        PlayerShip playerShip = PlayerManager.Inst.ActivePlayerShip;
        if (playerShip != null) playerShip.DisableSpecialFire();
    }

    private void MoveUp()
    {
        Debug.Log("[GameInputManager] MoveUp");
        UIManager.Inst.HandleMoveUp();
    }

    private void MoveDown()
    {
        Debug.Log("[GameInputManager] MoveDown");
        UIManager.Inst.HandleMoveDown();
    }

    private void MoveLeft()
    {
        Debug.Log("[GameInputManager] MoveLeft");
        UIManager.Inst.HandleMoveLeft();
    }

    private void MoveRight()
    {
        Debug.Log("[GameInputManager] MoveRight");
        UIManager.Inst.HandleMoveRight();
    }

    private void Select()
    {
        Debug.Log("[GameInputManager] Select");
        UIManager.Inst.HandleSelect();
    }
}

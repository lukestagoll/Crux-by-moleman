using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputHandler : MonoBehaviour
{
    private GameControls controls;

    private void Awake()
    {
        controls = new GameControls();
        controls.Gameplay.Pause.performed += _ => TogglePause();
        controls.Gameplay.PrimaryAttack.performed += ctx => OnPrimaryAttackPerformed();
        controls.Gameplay.PrimaryAttack.canceled += ctx => OnPrimaryAttackCanceled();
        controls.Gameplay.SpecialAttack.performed += ctx => OnSpecialAttackPerformed();
        controls.Gameplay.SpecialAttack.canceled += ctx => OnSpecialAttackCanceled();
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
        if (GameManager.IsPaused)
            PauseMenu.Inst.Resume();
        else
            PauseMenu.Inst.Pause();
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
        PlayerShip playerShip = PlayerManager.Inst.ActivePlayerShip;
        if (playerShip != null) playerShip.EnableSpecialFire();
    }

    private void OnSpecialAttackCanceled()
    {
        PlayerShip playerShip = PlayerManager.Inst.ActivePlayerShip;
        if (playerShip != null) playerShip.DisableSpecialFire();
    }
}

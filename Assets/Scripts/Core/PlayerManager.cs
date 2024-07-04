using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Game Data
    public static PlayerManager Inst { get; private set; }

    // Player Data
    public int Lives { get; set; }
    public PlayerShip ActivePlayerShip { get; set; }
    private AudioSource AudioSource;

    // Weapon States
    private bool PrimaryFireEnabled = false;
    private bool SpecialFireEnabled = false;
    private bool SpecialFireCeasing = false;

    // Store active weapon prefabs
    private List<string> ActiveWeaponPrefabs = new List<string>();

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("PlayerManager already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;
        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameManager.IsPaused) return;
        if (PrimaryFireEnabled) ActivePlayerShip.FireWeapons(WeaponType.Primary);
    }

    public void EnablePrimaryFire()
    {
        if (ActivePlayerShip == null || SpecialFireEnabled) return;
        if (!ActivePlayerShip.HasActiveAttachPoint(WeaponType.Primary)) return;
        PrimaryFireEnabled = true;
    }
    public void DisablePrimaryFire()
    {
        if (ActivePlayerShip == null || !PrimaryFireEnabled) return;
        PrimaryFireEnabled = false;
    }
    public void EnableSpecialFire()
    {
        if (ActivePlayerShip == null || SpecialFireEnabled) return;
        if (!ActivePlayerShip.HasActiveAttachPoint(WeaponType.Special)) return;
        DisablePrimaryFire();
        ActivePlayerShip.FireWeapons(WeaponType.Special);
        SpecialFireEnabled = true;
    }
    public void DisableSpecialFire()
    {
        if (!SpecialFireEnabled || SpecialFireCeasing) return;
        if (!ActivePlayerShip.HasActiveAttachPoint(WeaponType.Special))
        {
            HandleSpecialFireCeased();
            return;
        };
        SpecialFireCeasing = true;
        ActivePlayerShip.CeaseFire(WeaponType.Special);
    }
    public void HandleSpecialFireCeased()
    {
        SpecialFireCeasing = false;
        SpecialFireEnabled = false;
    }

    public void IncrementLives(int amt)
    {
        Lives += amt;
        HUDManager.Inst.UpdateLivesDisplay();
    }

    private void PlayExplosionSound()
    {
        if(AudioSource != null)
        {
            AudioSource.Play();
        }
    }

    public void SpawnPlayer()
    {
        ActivePlayerShip = Instantiate(AssetManager.PlayerPrefab, new Vector3(0, -4, 10), Quaternion.identity);
        PrimaryFireEnabled = false;
        SpecialFireEnabled = false;
        SpecialFireCeasing = false;
        // Reattach saved weapon prefabs
        // foreach (string weaponPrefabName in ActiveWeaponPrefabs)
        // {
        //     GameObject weaponPrefab = AssetManager.GetWeaponPrefab(weaponPrefabName);
        //     if (weaponPrefab != null)
        //     {
        //         ActivePlayerShip.AttemptWeaponAttachment(weaponPrefab, true);
        //     }
        // }

            GameObject weaponPrefab = AssetManager.GetWeaponPrefab("ElectroShield");
            if (weaponPrefab != null)
            {
                ActivePlayerShip.AttemptWeaponAttachment(weaponPrefab, false);
            }
    }

    public void HandlePlayerDestroyed()
    {
        Debug.Log("HandlePlayerDestroyed");
        PlayExplosionSound();
        DisablePrimaryFire();
        DisableSpecialFire();
        ActivePlayerShip.DisableShooting();
        // Save active weapon prefabs
        ActiveWeaponPrefabs.Clear();
        foreach (WeaponSlot weaponSlot in ActivePlayerShip.WeaponSlots)
        {
            foreach (AttachPoint attachPoint in weaponSlot.AttachPoints)
            {
                if (!attachPoint.IsEmpty)
                {
                    WeaponBase weapon = attachPoint.AttachedWeapon.GetComponent<WeaponBase>();
                    if (weapon != null)
                    {
                        string weaponPrefabName = weapon.name.Replace("(Clone)", "").Trim();
                        ActiveWeaponPrefabs.Add(weaponPrefabName);
                    }
                }
            }
        }

        Lives -= 1;
        ActivePlayerShip = null;
        if (Lives > 0)
        {
            HUDManager.Inst.UpdateLivesDisplay();
            RespawnPlayer();
        }
        else
        {
            // Game Over
            GameManager.HandleGameOver();
        }
    }

    public void SetActiveShipToNull()
    {
        ActivePlayerShip = null;
    }

    // Coroutine to delay the respawn
    private IEnumerator DelayedRespawn(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SpawnPlayer();
    }

    // Function called when the player is destroyed
    public void RespawnPlayer()
    {
        Debug.Log("Player destroyed. Respawning in 2 seconds...");
        StartCoroutine(DelayedRespawn(GameConfig.RespawnTimer)); // 2 seconds delay
    }
}

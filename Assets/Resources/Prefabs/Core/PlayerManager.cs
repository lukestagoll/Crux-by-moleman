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

    // Store active weapon prefabs
    // ! THIS WILL BE REPLACED BY EQUIPMENT AND LOADOUT
    private List<string> ActiveWeaponPrefabs = new List<string>();

    // Store Unlocked Skills
    private List<SkillBase> ActiveSkills = new List<SkillBase>();

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

    public void HandlePlayerDestroyed()
    {
        PlayExplosionSound();
        ActivePlayerShip.DisablePrimaryFire();
        ActivePlayerShip.DisableSpecialFire();
        ActivePlayerShip.DisableShooting();
        SetActiveWeapons();
        SetActiveShipToNull();

        Lives -= 1;
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

    private void SetActiveWeapons()
    {
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
    }

    // Function called when the player is destroyed
    public void RespawnPlayer()
    {
        Debug.Log("Player destroyed. Respawning in 2 seconds...");
        StartCoroutine(DelayedRespawn(GameConfig.RespawnTimer)); // 2 seconds delay
    }

    // Coroutine to delay the respawn
    private IEnumerator DelayedRespawn(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        ActivePlayerShip = Instantiate(AssetManager.PlayerPrefab, new Vector3(0, -4, 10), Quaternion.identity);
        // Do Skill things (modifiers/spawn loops)
        // Each skill should handle their own by listening for onSpawn event.
        InitialiseSkills();
        // Reattach saved weapon prefabs
        ReattachWeapons();
    }

    private void InitialiseSkills()
    {
        InitialShipData initialPlayerData = GameConfig.GetInitialPlayerData();
        if (initialPlayerData == null) {
            Debug.LogError("[PlayerManager] Failed to fetch InitialPlayerData");
            return;
        }
        // var shipSkillManager = ActivePlayerShip.GetComponent<ShipSkillManager>();
        ActiveSkills = ShipSkillManager.Initialise(initialPlayerData);
    }

    private void ReattachWeapons()
    {
        foreach (string weaponPrefabName in ActiveWeaponPrefabs)
        {
            GameObject weaponPrefab = AssetManager.GetWeaponPrefab(weaponPrefabName);
            if (weaponPrefab != null)
            {
                ActivePlayerShip.AttemptWeaponAttachment(weaponPrefab, true);
            }
        }
    }

    private void PlayExplosionSound()
    {
        if(AudioSource != null)
        {
            AudioSource.Play();
        }
    }

    public void IncrementLives(int amt)
    {
        Lives += amt;
        HUDManager.Inst.UpdateLivesDisplay();
    }

    public void SetActiveShipToNull()
    {
        ActivePlayerShip = null;
    }
}

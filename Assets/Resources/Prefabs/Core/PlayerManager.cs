using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Game Data
    public static PlayerManager Inst { get; private set; }

    // Player Data
    public int Lives { get; set; }
    public PlayerShip ActivePlayerShip { get; set; }
    private AudioSource AudioSource;

    // Relevant GameObjects
    public GameObject BottomPlayerBoundary;

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
        _ = SpawnPlayerAsync();
    }

    public async Task SpawnPlayerAsync(bool initialSpawn = false)
    {
        Vector3 spawnPosition = initialSpawn ? new Vector3(0, -6, 10) : new Vector3(0, -4, 10);

        ActivePlayerShip = Instantiate(AssetManager.PlayerPrefab, spawnPosition, Quaternion.identity);
        // Sets the players ship for each still and attemps activation (if not already)
        ShipSkillManager.AssignShipToSkills(ActiveSkills, ActivePlayerShip);
        // Reattach saved weapon prefabs
        ReattachWeapons();

        if (initialSpawn)
        {
            BottomPlayerBoundary.SetActive(false);
            var tcs = new TaskCompletionSource<bool>();
            StartCoroutine(MoveToPositionWithDeceleration(ActivePlayerShip.transform, new Vector3(0, -3, 10), 3.0f, 0.75f, tcs));
            await tcs.Task; // Wait for the movement to complete
        }
    }

    private IEnumerator MoveToPositionWithDeceleration(Transform transform, Vector3 targetPosition, float initialSpeed, float decelerationDistance, TaskCompletionSource<bool> tcs)
    {
        float currentSpeed = initialSpeed;
    
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceRemaining = Vector3.Distance(transform.position, targetPosition);
    
            // If within deceleration distance, reduce speed
            if (distanceRemaining < decelerationDistance)
            {
                currentSpeed = Mathf.Lerp(0, initialSpeed, distanceRemaining / decelerationDistance);
            }
    
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            yield return null;
        }
    
        BottomPlayerBoundary.SetActive(true);
        tcs.SetResult(true); // Signal that the movement is complete
    }


    public void BuildInitialSkills()
    {
        InitialShipData initialPlayerData = GameConfig.GetInitialPlayerData();
        if (initialPlayerData == null)
        {
            Debug.LogError("[PlayerManager] Failed to fetch InitialPlayerData");
            return;
        }
        ActiveSkills = ShipSkillManager.BuildSkillList(initialPlayerData, null);
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
        if (AudioSource != null)
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

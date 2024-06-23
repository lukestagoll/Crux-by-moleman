using System.Collections.Generic;
using UnityEngine;

public static class AssetManager
{
    // PREFABS
    public static PlayerShip PlayerPrefab { get; private set; }
    public static Wave WavePrefab { get; private set; }
    public static ItemDrop ItemDropPrefab { get; private set; }
    public static GameObject LifeIconPrefab { get; private set; }
    public static GameObject StarMapPrefab { get; private set; }
    public static GameObject DistantStarPrefab { get; private set; }
    public static GameObject DistantPlanetPrefab { get; private set; }
    public static GameObject PlanetPrefab { get; private set; }

    // PREFAB DICTIONARIES
    public static Dictionary<string, EnemyShip> EnemyShipPrefabs { get; private set; } = new Dictionary<string, EnemyShip>();
    public static Dictionary<string, GameObject> WeaponPrefabs { get; private set; } = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> ProjectilePrefabs { get; private set; } = new Dictionary<string, GameObject>();

    // SPRITES
    public static List<Sprite> DistantStarSprites { get; private set; } = new List<Sprite>();
    public static List<Sprite> DistantPlanetSprites { get; private set; } = new List<Sprite>();
    public static List<Sprite> PlanetSprites { get; private set; } = new List<Sprite>();

    // SETTINGS
    private static List<string> WeaponPrefabsToLoad = new List<string> { "Cannon", "CannonSmall", "MissileLauncher", "HomingMissileLauncher", "ElectroShield", "ElectroShieldEffect" };
    private static List<string> ProjectilesToLoad = new List<string> { "Plasma", "PlasmaLight", "Missile", "HomingMissile", "ElectricExplosion" };
    private static List<string> EnemyShipsToLoad = new List<string> { "SF1", "SF2" };

    // Fetch function for EnemyShipPrefabs
    public static EnemyShip GetEnemyShipPrefab(string key)
    {
        if (EnemyShipPrefabs.TryGetValue(key, out var prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"EnemyShip prefab with key '{key}' not found.");
            return null;
        }
    }

    // Fetch function for WeaponPrefabs
    public static GameObject GetWeaponPrefab(string key)
    {
        if (WeaponPrefabs.TryGetValue(key, out var weaponPrefabComponent))
        {
            return weaponPrefabComponent;
        }
        else
        {
            Debug.LogError($"Weapon prefab with key '{key}' not found.");
            return null;
        }
    }

    // Fetch function for ProjectilePrefabs
    public static GameObject GetProjectilePrefab(string key)
    {
        if (ProjectilePrefabs.TryGetValue(key, out var prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"Projectile prefab with key '{key}' not found.");
            return null;
        }
    }


    public static void CacheAssets()
    {
        // SHIPS
        PlayerPrefab = Resources.Load<PlayerShip>("Prefabs/Ships/Player");
        foreach (string shipName in EnemyShipsToLoad)
        {
            EnemyShip shipPrefab = Resources.Load<EnemyShip>($"Prefabs/Ships/{shipName}");
            if (shipPrefab != null)
            {
                EnemyShipPrefabs[shipName] = shipPrefab;
            }
            else
            {
                Debug.LogError($"Failed to load EnemyShip {shipName} prefab!");
            }
        }
        if (PlayerPrefab == null)
        {
            Debug.LogError("Failed to load Player prefab!");
        }

        // WEAPONS
        foreach (string weaponName in WeaponPrefabsToLoad)
        {
            GameObject weaponPrefab = Resources.Load<GameObject>($"Prefabs/Combat/Weapons/{weaponName}");
            if (weaponPrefab != null)
            {
                WeaponPrefabs[weaponName] = weaponPrefab;
            }
            else
            {
                Debug.LogError($"Failed to load Weapon {weaponName} prefab!");
            }
        }

        // PROJECTILES
        foreach (string projectileName in ProjectilesToLoad)
        {
            GameObject projectilePrefab = Resources.Load<GameObject>($"Prefabs/Combat/Projectiles/{projectileName}");
            if (projectilePrefab != null)
            {
                ProjectilePrefabs[projectileName] = projectilePrefab;
            }
            else
            {
                Debug.LogError($"Failed to load {projectileName} prefab!");
            }
        }

        // CORE
        WavePrefab = Resources.Load<Wave>("Prefabs/Game/Wave");
        if (WavePrefab == null)
        {
            Debug.LogError("Failed to load Wave prefab!");
        }

        // GAME
        ItemDropPrefab = Resources.Load<ItemDrop>("Prefabs/Game/ItemDrop");
        if (ItemDropPrefab == null)
        {
            Debug.LogError("Failed to load ItemDrop prefab!");
        }

        // UI
        StarMapPrefab = Resources.Load<GameObject>("Prefabs/UI/StarMap");
        LifeIconPrefab = Resources.Load<GameObject>("Prefabs/UI/ShipIcon");
        if (LifeIconPrefab == null)
        {
            Debug.LogError("Failed to load ShipIcon prefab!");
        }
        if (StarMapPrefab == null)
        {
            Debug.LogError("Failed to load StarMap prefab!");
        }

        // Distant Object
        DistantStarPrefab = Resources.Load<GameObject>("Prefabs/UI/DistantStar");
        DistantPlanetPrefab = Resources.Load<GameObject>("Prefabs/UI/DistantPlanet");
        if (DistantStarPrefab == null)
        {
            Debug.LogError("Failed to load DistantStarPrefab!");
        }
        if (DistantPlanetPrefab == null)
        {
            Debug.LogError("Failed to load DistantPlanetPrefab!");
        }

        PlanetPrefab = Resources.Load<GameObject>("Prefabs/UI/Planet");
        if (PlanetPrefab == null)
        {
            Debug.LogError("Failed to load PlanetPrefab!");
        }

        // Load all sprites from the specified directories
        DistantStarSprites.AddRange(Resources.LoadAll<Sprite>("Sprites/SPACE/DistantStars"));
        DistantPlanetSprites.AddRange(Resources.LoadAll<Sprite>("Sprites/SPACE/DistantPlanets"));
        PlanetSprites.AddRange(Resources.LoadAll<Sprite>("Sprites/SPACE/Planets"));
        if (DistantStarSprites.Count == 0)
        {
            Debug.LogError("No distant star sprites found in the specified directory.");
        }

        if (DistantPlanetSprites.Count == 0)
        {
            Debug.LogError("No distant planet sprites found in the specified directory.");
        }
        if (PlanetSprites.Count == 0)
        {
            Debug.LogError("No planet sprites found in the specified directory.");
        }
    }
}
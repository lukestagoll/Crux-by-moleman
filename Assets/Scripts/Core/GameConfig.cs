using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameConfig
{
    // GAME DATA
    public static bool HasBeenLoaded { get; private set; }
    public static GameData GameData { get; private set; }
    public static EnemyPaths EnemyPaths { get; private set; }
    public static Positions Positions { get; private set; }

    // PREFABS
    public static Dictionary<string, EnemyShip> EnemyShipPrefabs { get; private set; } = new Dictionary<string, EnemyShip>();
    public static ProjectileBase PlasmaPrefab { get; private set; }
    public static PlayerShip PlayerPrefab { get; private set; }
    public static Wave WavePrefab { get; private set; }
    public static GameObject LifeIconPrefab { get; private set; }
    public static GameObject StarMapPrefab { get; private set; }
    public static GameObject DistantStarPrefab { get; private set; }
    public static GameObject DistantPlanetPrefab { get; private set; }
    public static GameObject PlanetPrefab { get; private set; }

    // SPRITES
    public static List<Sprite> DistantStarSprites { get; private set; } = new List<Sprite>();
    public static List<Sprite> DistantPlanetSprites { get; private set; } = new List<Sprite>();
    public static List<Sprite> PlanetSprites { get; private set; } = new List<Sprite>();

    // CONFIG
    public static float MaxPlayerHealth = 200;
    public static int InitialLives = 3;
    public static int InitialScore = 0;
    public static float RespawnTimer = 2;

    public static void Initialise()
    {
        LoadGameData();
        LoadEnemyPaths();
        LoadPositions();
        CachePrefabs();
        HasBeenLoaded = true;
    }

    private static void LoadGameData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("gameData");
        if (jsonData == null)
        {
            Debug.LogError("Failed to load game data!");
            return;
        }

        GameData = JsonUtility.FromJson<GameData>(jsonData.text);
    }

    private static void LoadEnemyPaths()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("enemyPaths");
        if (jsonData == null)
        {
            Debug.LogError("Failed to load enemy paths data!");
            return;
        }

        EnemyPaths = JsonUtility.FromJson<EnemyPaths>(jsonData.text);

        if (EnemyPaths == null)
        {
            Debug.LogError("Failed to deserialize EnemyPaths.");
        }
    }

    private static void LoadPositions()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("positionToCoordinates");
        if (jsonData == null)
        {
            Debug.LogError("Failed to load position to coordinates data!");
            return;
        }
    
        Positions = JsonUtility.FromJson<Positions>(jsonData.text);
    
        if (Positions == null)
        {
            Debug.LogError("Failed to deserialize Positions.");
        }
    }

    private static void CachePrefabs()
    {
        // SHIPS
        PlayerPrefab = Resources.Load<PlayerShip>("Prefabs/Ships/Player");
        EnemyShipPrefabs.Add("SF1", Resources.Load<EnemyShip>("Prefabs/Ships/SF1"));
        EnemyShipPrefabs.Add("SF2", Resources.Load<EnemyShip>("Prefabs/Ships/SF2"));
        if (EnemyShipPrefabs["SF1"] == null)
        {
            Debug.LogError("Failed to load EnemyShip SF1 prefab!");
        }
        if (EnemyShipPrefabs["SF2"] == null)
        {
            Debug.LogError("Failed to load EnemyShip SF2 prefab!");
        }
        if (PlayerPrefab == null)
        {
            Debug.LogError("Failed to load Player prefab!");
        }

        // PROJECTILES
        PlasmaPrefab = Resources.Load<ProjectileBase>("Prefabs/Combat/Projectiles/Plasma");
        if (PlasmaPrefab == null)
        {
            Debug.LogError("Failed to load PlasmaPrefab!");
        }

        // CORE
        WavePrefab = Resources.Load<Wave>("Prefabs/Game/Wave");
        if (WavePrefab == null)
        {
            Debug.LogError("Failed to load Wave prefab!");
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

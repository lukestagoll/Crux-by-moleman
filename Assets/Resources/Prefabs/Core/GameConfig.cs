using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class GameConfig
{
    // GAME DATA
    public static bool HasBeenLoaded { get; private set; }

    public static GameData GameData { get; private set; }
    public static EnemyPaths EnemyPaths { get; private set; }
    public static Positions Positions { get; private set; }
    public static Dictionary<string, PathData> EnemyPathPresets { get; private set; } = new Dictionary<string, PathData>();
    public static List<EffectData> EffectDataList = new List<EffectData>();

    // CONFIG
    public static float MaxPlayerHealth = 100;
    public static float MaxPlayerShield = 150;
    public static int InitialLives = 3;
    public static int InitialScore = 0;
    public static float RespawnTimer = 2;

    public static void Initialise()
    {
        LoadGameData();
        LoadEnemyPaths();
        LoadEnemyPathPresets(); // New method
        LoadPositions();
        LoadEffectData();
        AssetManager.CacheAssets();
        HasBeenLoaded = true;
        Debug.Log("Game Config Loaded");
        
    }

    public static EffectData FetchEffectDataBySubType(EffectSubType subType)
    {
        return EffectDataList.Find(v => v.SubType == subType);
    }

    private static void LoadEffectData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("effectData");
        if (jsonData == null)
        {
            Debug.LogError("Failed to load effect data!");
            return;
        }
    
        EffectDataList = JsonConvert.DeserializeObject<List<EffectData>>(jsonData.text);
    
        if (EffectDataList == null || EffectDataList.Count == 0)
        {
            Debug.LogError("Failed to deserialize EffectData.");
            return;
        }
    }

    private static void LoadGameData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("gameData");
        if (jsonData == null)
        {
            Debug.LogError("Failed to load game data!");
            return;
        }

        GameData = JsonConvert.DeserializeObject<GameData>(jsonData.text);
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

    private static void LoadEnemyPathPresets()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("enemyPathPresets");
        if (jsonData == null)
        {
            Debug.LogError("Failed to load enemy path presets data!");
            return;
        }
        List<PathData> pathPresets = JsonConvert.DeserializeObject<List<PathData>>(jsonData.text);

        foreach (PathData preset in pathPresets)
        {
            EnemyPathPresets[preset.name] = preset;
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

}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Inst { get; private set; }
    public Slider healthBar;
    public TextMeshProUGUI ScoreDisplay;
    public Transform LivesDisplay;
    private float LifeIconSpacing = 35f;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("HUDManager already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;
    }

    void Start()
    {
        healthBar.maxValue = 100; // percentage
        UpdateLivesDisplay();
    }

    public void UpdateHealthBar(float currentHitpoints)
    {
        float percentage = currentHitpoints / GameConfig.MaxPlayerHealth * 100;
        healthBar.value = percentage;
    }

    public void UpdateScoreDisplay()
    {
        ScoreDisplay.text = $"{GameManager.Score}"; 
    }

    public void UpdateLivesDisplay()
    {
        int diff = LivesDisplay.childCount - PlayerManager.Inst.Lives;
        if (diff < 0)
        {
            for (int i = 0; i < -diff; i++)
            {
                GameObject img = Instantiate(AssetManager.LifeIconPrefab, LivesDisplay);
                img.transform.localPosition = new Vector3(LivesDisplay.childCount * LifeIconSpacing, 0, 0);
            }
        }
        else if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                Destroy(LivesDisplay.GetChild(LivesDisplay.childCount - 1).gameObject);
            }
        }
    }
}

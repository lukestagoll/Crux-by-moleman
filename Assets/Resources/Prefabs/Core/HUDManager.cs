using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Inst { get; private set; }
    public Slider healthBar;
    public Slider shieldBar;
    public TextMeshProUGUI ScoreDisplay;
    public Transform LivesDisplay;
    private float LifeIconSpacing = 35f;

    private Coroutine scoreUpdateCoroutine;
    private float currentDisplayedScore;


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
        shieldBar.maxValue = 100; // percentage
        UpdateLivesDisplay();
    }

    public void UpdateHealthBar(float amt)
    {
        float percentage = amt / GameConfig.MaxPlayerHealth * 100;
        healthBar.value = percentage;
    }

    public void UpdateShieldBar(float amt)
    {
        float percentage = amt / GameConfig.MaxPlayerShield * 100;
        shieldBar.value = percentage;
    }

    public void UpdateScoreDisplay()
    {
        if (scoreUpdateCoroutine != null)
        {
            StopCoroutine(scoreUpdateCoroutine);
        }
        scoreUpdateCoroutine = StartCoroutine(AnimateScoreChange());
    }

    private IEnumerator AnimateScoreChange()
    {
        float targetScore = GameManager.Score;
        float animationDuration = Mathf.Clamp(Mathf.Abs(targetScore - currentDisplayedScore) / 100f, 0.5f, 2f);
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            currentDisplayedScore = Mathf.Lerp(currentDisplayedScore, targetScore, t);
            ScoreDisplay.text = Mathf.RoundToInt(currentDisplayedScore).ToString();
            yield return null;
        }

        currentDisplayedScore = targetScore;
        ScoreDisplay.text = targetScore.ToString();
        scoreUpdateCoroutine = null;
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

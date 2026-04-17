using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public float currentScore = 0f;
    public SaveData data;
    public bool isPlaying = false;
    public UnityEvent onPlay = new();
    public UnityEvent onGameOver = new();
    public int currentPhase = 1;
    public UnityEvent<int> onPhaseChange = new();

    public SpriteRenderer backgroundRenderer;
    public SpriteRenderer celestialRenderer; // The Sun/Moon Sprite

    private Color[] phaseColors = new Color[] {
        Color.white,                 // Phase 1: Normal (no tint)
        new Color(0.8f, 0.4f, 0.2f), // Phase 2: orange/dusk
        new Color(0.2f, 0.2f, 0.4f), // Phase 3: dark night
        new Color(0.5f, 0.1f, 0.1f), // Phase 4+: red
    };

    private Color[] celestialColors = new Color[] {
        new Color(1f, 0.9f, 0.2f),   // Phase 1: Sun (Bright Yellow)
        new Color(1f, 0.4f, 0.1f),   // Phase 2: Setting Sun (Orange/Red)
        new Color(0.8f, 0.9f, 1f),   // Phase 3: Moon (Pale Blue)
        new Color(0.6f, 0.0f, 0.0f), // Phase 4+: Blood Moon (Dark Red)
    };

    private void Start()
    {
        data = new SaveData();
    }

    private void Update()
    {
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
            CheckPhase();
        }
    }

    private void CheckPhase()
    {
        int newPhase = 1 + Mathf.FloorToInt(currentScore / 15f);
        if(newPhase > currentPhase)
        {
            currentPhase = newPhase;
            onPhaseChange.Invoke(currentPhase);
            if (backgroundRenderer != null)
            {
                int colorIndex = Mathf.Min(currentPhase - 1, phaseColors.Length - 1);
                backgroundRenderer.color = phaseColors[colorIndex];
            }
            if (celestialRenderer != null)
            {
                int colorIndex = Mathf.Min(currentPhase - 1, celestialColors.Length - 1);
                celestialRenderer.color = celestialColors[colorIndex];
            }
        }
    }
    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;
        Time.timeScale = 1f;
        currentScore = 0;
        currentPhase = 1; // Reset phase
        if (backgroundRenderer != null)
        {
            backgroundRenderer.color = phaseColors[0];
        }
        if (celestialRenderer != null)
        {
            celestialRenderer.color = celestialColors[0];
        }
    }
    public void GameOver()
    {
        data.highscore = PlayerPrefs.GetFloat("highscore", 0f);
        if (currentScore > data.highscore)
        {
            data.highscore = currentScore;
            PlayerPrefs.SetFloat("highscore", data.highscore);
            PlayerPrefs.Save();
        }
        isPlaying = false;
        onGameOver.Invoke();
    }
    public string PrettyScore => Mathf.RoundToInt(currentScore).ToString();
    public string PrettyHighscore => Mathf.RoundToInt(PlayerPrefs.GetFloat("highscore")).ToString();
}

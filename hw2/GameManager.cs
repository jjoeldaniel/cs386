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

    private void Start()
    {
        data = new SaveData();
    }

    private void Update()
    {
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }
    }
    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;
        Time.timeScale = 1f;
        currentScore = 0;
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

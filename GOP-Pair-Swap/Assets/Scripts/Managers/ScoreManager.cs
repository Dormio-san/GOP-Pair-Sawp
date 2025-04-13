using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Text assets for displaying score and high score
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    // Current score and high score variables
    private int currentScore = 0;
    private int highscore = 0;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of ScoreManager exists
        // Set Instance to this if it's null, otherwise destroy this object
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // Load high score from PlayerPrefs
            highscore = PlayerPrefs.GetInt("HighScore", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update the UI with whatever was loaded in from player prefs.
    private void Start()
    {
        UpdateUI();
    }

    // Set the score and check if it's a new high score
    public void SetScore(int newScore)
    {
        currentScore = newScore;

        if (currentScore > highscore)
        {
            highscore = currentScore;
            PlayerPrefs.SetInt("HighScore", highscore);
        }

        UpdateUI();
    }

    // Update the UI with the current score and high score
    private void UpdateUI()
    {
        if (currentScoreText != null)
            currentScoreText.text = $"Score: {currentScore}";

        if (highscoreText != null)
            highscoreText.text = $"Highscore: {highscore}";
    }

    // Reset the current score to 0 and update the UI
    // Typically used on game restart if it isn't already 0
    public void ResetScore()
    {
        currentScore = 0;
        UpdateUI();
    }
}
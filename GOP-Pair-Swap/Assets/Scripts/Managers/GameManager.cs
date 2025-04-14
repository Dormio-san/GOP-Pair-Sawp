using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameOverUI gameOverUI; // Reference to the Game Over UI script
    [SerializeField] private GameObject pauseUI; // Reference to the pause UI script
    private PauseUI pauseUIScript; // Reference to the Pause UI script
    [HideInInspector] public bool isPaused = false;

    private void Start()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        // Set Instance to this if it's null, otherwise destroy this object
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Set the reference to the pause UI script
        pauseUIScript = pauseUI.GetComponent<PauseUI>();

        // By default, make the cursor invisible and lock it to the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (InputManager.Instance.PauseInput)
        {
            if (!isPaused)
            {
                // If the game is not paused, pause it
                PauseGame();
            }
            else
            {
                // If the game is paused, resume it
                pauseUIScript.ResumeGame();
            }
        }
    }

    // Called when the game ends
    public void GameOver()
    {
        gameOverUI.ShowGameOverUI();
    }

    // Called when the game is paused
    public void PauseGame()
    {
        isPaused = true;
        pauseUI.SetActive(true);
        pauseUIScript.GamePaused();
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameOverUI gameOverUI; // Reference to the Game Over UI script

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

        // By default, make the cursor invisible and lock it to the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Called when the game ends
    public void GameOver()
    {
        gameOverUI.ShowGameOverUI();
    }
}
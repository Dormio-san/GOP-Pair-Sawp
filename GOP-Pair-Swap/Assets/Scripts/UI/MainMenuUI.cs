using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f; // Ensure the game time is running at the start

        // Ensure player can use their cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void BeginPlay()
    {
        // Load the first level of the game
        SceneManager.LoadScene(1);
    }

    // Quit the game
    public void QuitJaywalking()
    {
#if UNITY_EDITOR
        // If in the editor, stop playing the game
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // If in the build, quit the application
        Application.Quit();
    }
}
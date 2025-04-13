using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] private CanvasGroup canvasGroup; // The canvas group for the game over UI
    private float fadeDuration = 2f; // The duration of the fade in effect

    [SerializeField] private GameObject defaultSelectedButton; // The default button to select when the game over UI is shown

    private void Start()
    {
        // Set the canvas group to be invisible at the start
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        // Show the game over UI and start the fade in effect
        canvasGroup.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Fade in the canvas group
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Set the canvas group to be fully visible and interactable
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Give the player usage of their cursor.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // Deselect any currently selected object and select the default button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);

        // Stop the game time to freeze all updates and coroutines
        Time.timeScale = 0f;
    }

    // Start the level over
    public void RestartLevel()
    {
        // Resume the game time or else the new game will be frozen
        Time.timeScale = 1f;

        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Stop jaywalking
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
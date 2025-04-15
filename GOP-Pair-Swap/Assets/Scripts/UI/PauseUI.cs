using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private CanvasGroup canvasGroup; // The canvas group for the pause UI
    private float fadeDuration = 0.5f; // The duration of the fade in effect

    [SerializeField] private GameObject defaultSelectedButton; // The default button to select when the pause menu is shown

    public void GamePaused()
    {
        // Stop the game time to freeze all updates and coroutines
        Time.timeScale = 0f;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Fade in the canvas group
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
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
    }

    public void ResumeGame()
    {
        // Resume the game time
        Time.timeScale = 1f;

        // Hide the pause menu
        gameObject.SetActive(false);

        // Hide the cursor and lock it to the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.isPaused = false; // Set the pause state to false
    }

    public void MainMenu()
    {
        // Resume the game time
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
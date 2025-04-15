using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    private float moveTime = 0.1f; // How long it will take to move between tiles
    private bool isMoving = false;
    private Vector3 targetPosition;
    private float minX; // Min x screen boundary
    private float maxX; // Max x screen boundary
    private bool canMove = true; // Whether the player can move or not

    // The maximum number of moves the player can make downwards
    // This prevents the player from moving outside of the map
    private int maxNumOfDownwardsMoves = 6;

    // Variables related to calculating the player's score
    [Header("Score")]
    private int highestRowReached = 0; // The highest row the player has reached
    private int score = 0; // The player's score

    [Header("Explosion Animation")]
    [SerializeField] private GameObject explosionPrefab; // The explosion prefab

    // Variables related to the logs and water tiles
    private bool isOnLog = false; // Whether the player is on a log or not
    private float drownDelay = 0.15f; // Short delay before the player drowns (prevents drowning when jumping to a new log)
    private Transform currentLog;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb2D;

    private void Start()
    {
        // Set the screen bounds used for when the player goes off screen
        minX = Mathf.Round(ScreenBounds.Instance.Left) + 0.5f;
        maxX = Mathf.Round(ScreenBounds.Instance.Right) - 0.5f;
        //Debug.Log($"MinX: {minX}, MaxX: {maxX}");
    }

    void Update()
    {
        // If already moving or can't move, return
        if (isMoving || !canMove) return;

        Vector2 direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W))
            direction = Vector2.up;

        else if (Input.GetKeyDown(KeyCode.S))
            direction = Vector2.down;

        else if (Input.GetKeyDown(KeyCode.A))
            direction = Vector2.left;

        else if (Input.GetKeyDown(KeyCode.D))
            direction = Vector2.right;

        // If the player pressed a movement key, check for some other things before moving
        if (direction != Vector2.zero)
        {
            targetPosition = transform.position + (Vector3)direction;
            SFXManager.Instance.PlayerMove();
            // If player goes off the left side of the screen, wrap around to the right
            if (targetPosition.x < minX)
            {
                if (isOnLog)
                {
                    // Player falls off the log into the water — game over
                    StartCoroutine(Drown());
                    return;
                }
                else
                {
                    targetPosition.x = maxX;
                    transform.position = targetPosition;
                }
            }
            // If player goes off the right side of the screen, wrap around to the left
            else if (targetPosition.x > maxX)
            {
                if (isOnLog)
                {
                    StartCoroutine(Drown());
                    return;
                }
                else
                {
                    targetPosition.x = minX;
                    transform.position = targetPosition;
                }
            }
            else
            {
                // Gets the Y position of the tile to move to
                int newYPos = Mathf.FloorToInt(targetPosition.y);

                // If player is going too far down, block downwards movement
                if (newYPos < highestRowReached - maxNumOfDownwardsMoves)
                    return;

                // Use smooth movement if able to move to new location
                StartCoroutine(SmoothMoveToTarget(targetPosition, newYPos));
            }
        }
    }

    // Smoothly move the player to the target position
    IEnumerator SmoothMoveToTarget(Vector3 target, int currentRow)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        // lerp the position of the player to the target position as long as time time spend moving so far is less than the allowed time to move
        while (elapsed < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, target, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isMoving = false;

        if (WorldRowManager.Instance.waterRowYPositions.Contains(currentRow) && !isOnLog)
        {
            // If the player is not on a log and they are on a water tile, drown
            StartCoroutine(Drown());
        }
        else if (isOnLog)
        {
            // If the player is on a log, move with the log
            yield return new WaitForSeconds(drownDelay); // Wait for a short delay before moving with the log
        }

        UpdateScore(currentRow); // Update score after moving
    }

    // Update score based on the player's row
    private void UpdateScore(int currentRow)
    {
        // Only update score if the player moves to a row they haven't reached yet
        if (currentRow > highestRowReached)
        {
            highestRowReached = currentRow;
            score = highestRowReached; // Set score to the highest row reached
            ScoreManager.Instance.SetScore(score);
        }
    }

    // When player dies, play the explosion animation
    public void PlayExplosionAnim()
    {
        canMove = false; // Disable player movement
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Set the player to not active
        gameObject.SetActive(false);
    }

    // Attach to the log when the player enters the trigger (called from log script)
    public void AttachToLog(Transform logAnchor)
    {
        isOnLog = true;
        currentLog = logAnchor;
        transform.SetParent(logAnchor);
        transform.position = logAnchor.position; // Set the player's position to the log anchor position
    }

    // Detach from the log when the player exits the trigger (called from log script)
    public void DetachFromLog(Transform logAnchor)
    {
        if (currentLog == logAnchor && canMove)
        {
            isOnLog = false;
            currentLog = null;
            transform.SetParent(null);
        }        
    }

    // When the player reaches the end of the screen on a log, they drown and game is over
    IEnumerator Drown()
    {
        rb2D.simulated = false; // Disable simulation of the rigidbody to prevent physics interactions
        canMove = false; // Disable player movement
        yield return new WaitForSeconds(drownDelay); // Wait for a short delay before drowning
        SFXManager.Instance.WaterDeath();
        GameManager.Instance.GameOver(); // Call the Game Over function
    }

    // Called from the log script when the player is on the edge of the screen
    public void EdgeOfScreenOnLog()
    {
        if (isOnLog && canMove)
            StartCoroutine(Drown()); // Start the drowning coroutine
    }
}
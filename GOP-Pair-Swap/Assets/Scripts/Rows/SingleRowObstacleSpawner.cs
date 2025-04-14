using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Text;

public class SingleRowObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform obstacleParent;

    private RowType rowType; // The type of row this spawner is attached to

    // The spawn rate for obstacles on different row types
    [Header("Obstacle Spawn Rates")]
    private float waterMinSpawnRate = 0.5f;
    private float waterMaxSpawnRate = 2.5f;
    private float roadMinSpawnRate = 2f;
    private float roadMaxSpawnRate = 3.75f;
    private float minSpawnRate = 0.1f;
    private float maxSpawnRate = 0.1f;

    // Speed of obstacles
    private float minObstacleSpeed = 2.5f;
    private float maxObstacleSpeed = 7.5f;
    private float obstacleSpeed;

    // Used to pool obstacles
    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    private float rowWidth; // Used to determine when an obstacle is offscreen

    // Obstacle has random spawn direction, so use these to say which direction movement is in and where to spawn
    private Vector3 moveDirection;
    private float spawnX;

    private void Start()
    {
        rowWidth = ScreenBounds.Instance.Width;
    }

    // When the object is enabled, start spawning obstacles
    private void OnEnable()
    {
        // Random speed and move direction
        obstacleSpeed = Random.Range(minObstacleSpeed, maxObstacleSpeed);
        bool moveTowardsLeft = Random.Range(0, 2) == 0;
        moveDirection = moveTowardsLeft ? Vector3.left : Vector3.right;
        spawnX = moveTowardsLeft ? ScreenBounds.Instance.Right + 1f : ScreenBounds.Instance.Left - 1f;

        StartCoroutine(SpawnObstaclesLoop());
    }

    // When the object is disabled, stop the coroutine and reset the obstacles
    private void OnDisable()
    {
        StopAllCoroutines();
        ResetObstacles();
    }

    private void FixedUpdate() //A for loop inside an update tick can become unoptimized over time. //Omar
    {
        UpdateObstacles();
    }

    // Set the row type and then call function to adjust spawn rate based on the row type
    public void SetRowType(RowType type)
    {
        rowType = type;
        SetSpawnRate();
    }

    // Set the spawn rate based on the row type
    private void SetSpawnRate()
    {
        switch (rowType)
        {
            case RowType.Water:
                minSpawnRate = waterMinSpawnRate;
                maxSpawnRate = waterMaxSpawnRate;
                break;
            case RowType.Road:
                minSpawnRate = roadMinSpawnRate;
                maxSpawnRate = roadMaxSpawnRate;
                break;
            default:
                minSpawnRate = 0.1f;
                maxSpawnRate = 0.1f;
                break;
        }
    }

    // Coroutine to spawn obstacles at random intervals
    private IEnumerator SpawnObstaclesLoop()
    {
        // Short delay before starting to spawn obstacles
        yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));

        while (true)
        {
            SpawnObstacle();

            // Wait for a random amount of time between the min and max spawn rate to add randomness to spawning.
            float waitTime = Random.Range(minSpawnRate, maxSpawnRate);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Spawn an obstacle
    private void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject obstacle;

        // Check if there are any inactive obstacles in the pool
        // If there are, dequeue one and return it to be used
        if (pool.Count > 0)
        {
            obstacle = pool.Dequeue();
            obstacle.SetActive(true);
        }
        else
        {
            // Pool is empty, so create a new obstacle
            obstacle = Instantiate(prefab, obstacleParent);
        }

        // Rotation Y and Z values for moving in right and left direction based on row types
        int rightRotationZ = -90;
        int leftRotationZ = 90;
        int rightRotationY = 0;
        int leftRotationY = 0;
        switch (rowType)
        {
            case RowType.Water:
                rightRotationZ = 0;
                leftRotationZ = 0;
                rightRotationY = 0;
                leftRotationY = 180;
                break;
            case RowType.Road:
                // Default roation for right direction is -90 degrees due to sprites pointing up by default
                rightRotationZ = -90;
                leftRotationZ = 90;
                rightRotationY = 0;
                leftRotationY = 0;
                break;
        }

        // By default, moving to right, so use the right rotation value
        obstacle.transform.rotation = Quaternion.Euler (0, rightRotationY, rightRotationZ);

        // If moving to the left, use the left roation value
        if (moveDirection == Vector3.left)
        {
            obstacle.transform.rotation = Quaternion.Euler(0, leftRotationY, leftRotationZ);
        }

        // Spawn obstacles at the edge of the screen and add it to the active obstacles list
        obstacle.transform.localPosition = new Vector3(spawnX, 0, 0);
        activeObstacles.Add(obstacle);
    }

    // Update the position of the obstacles
    private void UpdateObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            // Move the obstacle in the specified direction over time
            GameObject obj = activeObstacles[i];
            obj.transform.localPosition += moveDirection * obstacleSpeed * Time.deltaTime;

            // Determine if the obstacle is offscreen based on its direction
            bool isOffscreen = moveDirection.x < 0
                ? obj.transform.localPosition.x < ScreenBounds.Instance.Left - 1f
                : obj.transform.localPosition.x > ScreenBounds.Instance.Right + 1f;

            // If the obstacle is offscreen, deactivate it and return it to the pool
            if (isOffscreen)
            {
                // Check if obstacle is a log
                Log log = obj.GetComponent<Log>();

                // If a log, check if the player is on it
                if (log != null)
                {
                    log.CheckIfPlayerOnLog();
                }

                obj.SetActive(false);
                pool.Enqueue(obj);
                activeObstacles.RemoveAt(i);
            }
        }
    }

    // Reset all obstacles
    private void ResetObstacles()
    {
        // Deactivate all active obstacles and return them to the pool
        foreach (GameObject obj in activeObstacles)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        activeObstacles.Clear(); // Clear the list of active obstacles
    }
}
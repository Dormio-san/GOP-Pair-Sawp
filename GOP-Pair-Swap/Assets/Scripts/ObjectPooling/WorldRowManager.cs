using System.Collections.Generic;
using UnityEngine;

public class WorldRowManager : MonoBehaviour
{
    public static WorldRowManager Instance { get; private set; }
    [SerializeField] private WorldRowPooler pooler;
    [SerializeField] private Transform player;

    // Number of rows to spawn ahead and behind the player (ensures player always has rows to walk on)
    private readonly int rowsAhead = 10;
    private readonly int rowsBehind = 10;

    private int currentTopRowY = 0; // The current top row Y position. This is used to track the highest row spawned so far.
    private List<RowInfo> activeRows = new List<RowInfo>();

    // Use a hashset for faster lookup when checking if a row is water. Little more memory, but faster lookup speed than array or other alternatives.
    public HashSet<int> waterRowYPositions = new HashSet<int>();

    private void Start()
    {
        // Singleton pattern to ensure only one instance of WorldRowManager exists
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

        // Spawn initial rows
        for (int i = -rowsBehind; i <= rowsAhead; i++)
            SpawnRowAt(i);

        // Set the current top row to the last spawned row. This prevents duplicate rows from spawning.
        currentTopRowY = rowsAhead;
    }

    private void FixedUpdate() //Changed to Fixed Update as the player isnt moving that fast to warrant using normal Update //Omar
    {
        // Get the player current Y position and use it to calculate if we need to spawn or remove rows
        int playerRowY = Mathf.FloorToInt(player.position.y);
        int desiredTopY = playerRowY + rowsAhead;

        // Spawn new rows if the player has moved up
        while (currentTopRowY < desiredTopY)
        {
            currentTopRowY++;
            SpawnRowAt(currentTopRowY);
        }

        // Cleanup old rows when the player moves up because we don't want to keep rows that are far behind
        CleanupOldRows(playerRowY - rowsBehind);
    }

    void SpawnRowAt(int y)
    {
        RowType type;

        // The first three rows surrounding the player are always grass because it's a safe zone
        int safeZoneMaxHeight = 1; // How high up safe zone goes. Translates to -1, 0, and 1 as safe rows.
        if (y >= -safeZoneMaxHeight && y <= safeZoneMaxHeight)
            type = RowType.Grass;
        else
            type = GetRandomRowType();

        // Get a row from the pooler and set its position
        GameObject row = pooler.GetRow(type);
        row.transform.position = new Vector3(0.5f, y + 0.5f, 0);
        row.transform.SetParent(this.transform); //Made rows a child object of this transform to help the workflow when developing //Omar
        activeRows.Add(new RowInfo { y = y, type = type, row = row });

        // If row is water, add it to the waterRowYPositions HashSet
        if (type == RowType.Water)
            waterRowYPositions.Add(y);

        SingleRowObstacleSpawner obstacleSpawner = row.GetComponent<SingleRowObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            // Give the obstacle spawner its row type so it can vary the spawn rate based on type
            obstacleSpawner.SetRowType(type);
        }
    }

    void CleanupOldRows(int minY)
    {
        for (int i = activeRows.Count - 1; i >= 0; i--)
        {
            if (activeRows[i].y < minY)
            {
                // Remove the row from the waterRowYPositions HashSet if it was water
                if (activeRows[i].type == RowType.Water)
                    waterRowYPositions.Remove(activeRows[i].y);

                // Return the row to the pool
                pooler.ReturnRow(activeRows[i].type, activeRows[i].row);
                activeRows.RemoveAt(i);
            }
        }
    }

    // Returns a random row type based on the specified probabilities
    RowType GetRandomRowType()
    {
        int rand = Random.Range(0, 30);
        return rand switch
        {
            < 9 => RowType.Grass,
            < 21 => RowType.Road,
            _ => RowType.Water,
        };
    }

    struct RowInfo
    {
        public int y;
        public RowType type;
        public GameObject row;
    }
}
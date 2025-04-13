using System.Collections.Generic;
using UnityEngine;

public class WorldRowPooler : MonoBehaviour
{
    [SerializeField] private GameObject grassPrefab, roadPrefab, waterPrefab;

    private Dictionary<RowType, Queue<GameObject>> pool = new Dictionary<RowType, Queue<GameObject>>();

    void Awake()
    {
        // Loop through all RowType values and initialize the pool with empty queues for each type
        foreach (RowType type in System.Enum.GetValues(typeof(RowType)))
        {
            pool[type] = new Queue<GameObject>();
        }
    }

    // Method to get a row from the pool
    public GameObject GetRow(RowType type)
    {
        Queue<GameObject> selectedQueue = pool[type];

        // Check if there are any inactive rows in the selected queue
        // If there are, dequeue one and return it to be used
        if (selectedQueue.Count > 0)
        {
            GameObject row = selectedQueue.Dequeue();
            row.SetActive(true);
            return row;
        }

        // If pool is empty, create a new row
        GameObject prefab = type switch
        {
            RowType.Grass => grassPrefab,
            RowType.Road => roadPrefab,
            RowType.Water => waterPrefab,
            _ => null
        };

        return Instantiate(prefab);
    }

    // Method to return a row back into the pool
    public void ReturnRow(RowType type, GameObject row)
    {
        row.SetActive(false);
        pool[type].Enqueue(row);
    }
}

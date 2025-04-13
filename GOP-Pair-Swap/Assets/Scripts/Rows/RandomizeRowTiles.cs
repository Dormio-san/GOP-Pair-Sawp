using UnityEngine;

public class RandomizeRowTiles : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] rowTiles; // Tiles on this prefab
    [SerializeField] private Sprite[] randomSprites; // Array of sprites to choose from when randomizing

    void Start()
    {
        // For loop to iterate through each tile in the row, and choose a random sprite
        for (int i = 0; i < rowTiles.Length; i++)
        {
            // Choose a random sprite from the sprites array
            int randomIndex = Random.Range(0, randomSprites.Length);
            Sprite randomSprite = randomSprites[randomIndex];

            // Set the sprite of the tile to the random sprite
            rowTiles[i].sprite = randomSprite;
        }
    }
}
using UnityEngine;

public class RandomVehicleSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer vehicle; // The vehicle sprite renderer
    [SerializeField] private Sprite[] randomSprites; // Array of sprites to choose from when randomizing

    void OnEnable()
    {
        // Choose a random sprite from the sprites array
        int randomIndex = Random.Range(0, randomSprites.Length);
        Sprite randomSprite = randomSprites[randomIndex];

        // Set the sprite of the vehicle to the random sprite
        vehicle.sprite = randomSprite;
    }
}
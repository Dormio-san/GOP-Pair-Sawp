using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Player>()?.PlayExplosionAnim();

        // Call the GameOver method from GameManager
        GameManager.Instance.GameOver();
    }
}
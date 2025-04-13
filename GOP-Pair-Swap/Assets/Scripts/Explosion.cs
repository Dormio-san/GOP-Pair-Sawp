using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Called from the animation timeline to destroy the explosion object
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

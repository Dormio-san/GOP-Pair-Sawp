using UnityEngine;

public class Log : MonoBehaviour
{
    // The player will be attached to this point when on the log
    [SerializeField] private GameObject playerAttachPoint;

    private Player player; // Ref to player 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.AttachToLog(playerAttachPoint.transform);
            this.player = player; // set ref to player
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.DetachFromLog(playerAttachPoint.transform);
            this.player = null; // set ref to null
        }
    }

    // Function called when log reaches the edge of the screen to check if the player is on it
    public void CheckIfPlayerOnLog()
    {
        if (player != null)
        {
            player.transform.SetParent(null); // Detach player from log
            player.EdgeOfScreenOnLog();
            player = null; // set ref to null
        }
    }
}
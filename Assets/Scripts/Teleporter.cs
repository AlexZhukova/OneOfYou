using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportTarget; // The target position to teleport to
    public void Teleport() {  
        if (teleportTarget != null)
        {
            // Teleport the player to the target position
            transform.position = teleportTarget.position;
        }
        else
        {
            Debug.LogWarning("Teleport target is not set.");
        }
    }
}

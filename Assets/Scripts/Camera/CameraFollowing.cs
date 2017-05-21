using UnityEngine;


/// <summary>
/// Makes the camera follow the player if they are alive.
/// </summary>
public class CameraFollowing : MonoBehaviour {

    private PlayerController player;

    public void LateUpdate() {
        if (!player) {
            player = FindObjectOfType<PlayerController>();

            if (!player) {
                return;
            }
        }

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }

}

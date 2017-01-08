using UnityEngine;


/// <summary>
/// Makes the camera follow the player and the mouse.
/// </summary>
public class CameraFollowing : MonoBehaviour {

    public void LateUpdate() {
        var target = FindObjectOfType<PlayerController>();
        if (!target) {
            return;
        }

        var offset = new Vector2((Input.mousePosition.x - Screen.width / 2f) / Screen.width,
            (Input.mousePosition.y - Screen.height / 2f) / Screen.height);
        float maxOffset = 6f;

        transform.position = new Vector3(target.transform.position.x + offset.x * maxOffset,
            target.transform.position.y + offset.y * maxOffset, -10f);
    }

}

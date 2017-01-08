using UnityEngine;


/// <summary>
/// Makes the camera follow the player and the mouse.
/// </summary>
public class CameraFollowing : MonoBehaviour {

    public void Update() {
        var target = FindObjectOfType<PlayerController>();
        if (!target) {
            Debug.LogWarning("No player detected!");
            return;
        }

        var offset = new Vector2((Input.mousePosition.x - Screen.width / 2f) / Screen.width,
            (Input.mousePosition.y - Screen.height / 2f) / Screen.height);
        float roundAdjust = Globals.pixelsPerUnit;
        float maxOffset = 6f * roundAdjust;

        transform.position = new Vector3(Mathf.Round(target.transform.position.x * roundAdjust + offset.x * maxOffset) / roundAdjust,
            Mathf.Round(target.transform.position.y * roundAdjust + offset.y * maxOffset) / roundAdjust, -10f);
    }

}
